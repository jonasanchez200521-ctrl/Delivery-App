// Interop para renderizar el mapa de seguimiento de pedido con Leaflet + OpenStreetMap,
// con ruta real por calles calculada vía OSRM (servidor público de demostración, sin API key).
window.namiMap = {
    _maps: {},

    // showMoto: true cuando el pedido está en camino (PickedUp) -> anima un marcador de moto
    // recorriendo la ruta calculada. etaElementId: id de un elemento donde se escribe el
    // texto "Llega aprox. en X min" una vez que OSRM responde.
    render: function (elementId, etaElementId, restLat, restLng, restName, homeLat, homeLng, homeName, showMoto) {
        window.namiMap.dispose(elementId);

        const el = document.getElementById(elementId);
        if (!el || !window.L) return;

        const restLL = [restLat, restLng];
        const homeLL = [homeLat, homeLng];

        const map = window.L.map(el, { attributionControl: false, scrollWheelZoom: false });
        window.L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', { maxZoom: 19 }).addTo(map);

        // Línea recta de respaldo mientras se resuelve (o si falla) la ruta real por calles.
        let routeLine = window.L.polyline([restLL, homeLL], { color: '#EF9F27', weight: 3, dashArray: '6 8' }).addTo(map);

        window.L.circleMarker(restLL, { radius: 9, color: '#ffffff', weight: 3, fillColor: '#EF9F27', fillOpacity: 1 })
            .addTo(map).bindTooltip(restName, { permanent: true, direction: 'top', offset: [0, -10] });

        window.L.circleMarker(homeLL, { radius: 9, color: '#ffffff', weight: 3, fillColor: '#1a1a1f', fillOpacity: 1 })
            .addTo(map).bindTooltip(homeName, { permanent: true, direction: 'top', offset: [0, -10] });

        map.fitBounds(window.L.latLngBounds(restLL, homeLL).pad(0.3));

        const state = { map: map, motoTimer: null, motoMarker: null };
        window.namiMap._maps[elementId] = state;

        const etaEl = etaElementId ? document.getElementById(etaElementId) : null;
        if (etaEl) etaEl.textContent = 'Calculando ruta…';

        // Ruta real por calles vía OSRM. OSRM espera lng,lat (no lat,lng).
        const url = `https://router.project-osrm.org/route/v1/driving/${restLng},${restLat};${homeLng},${homeLat}?overview=full&geometries=geojson`;
        fetch(url)
            .then(r => r.json())
            .then(data => {
                if (window.namiMap._maps[elementId] !== state) return; // el componente ya se destruyó/re-renderizó
                if (data.code !== 'Ok' || !data.routes || !data.routes.length) throw new Error('sin ruta');

                const route = data.routes[0];
                const coords = route.geometry.coordinates.map(c => [c[1], c[0]]);

                map.removeLayer(routeLine);
                routeLine = window.L.polyline(coords, { color: '#EF9F27', weight: 5, opacity: 0.9 }).addTo(map);
                map.fitBounds(routeLine.getBounds().pad(0.2));
                state.routeLine = routeLine;

                if (etaEl) {
                    const minutes = Math.max(1, Math.round(route.duration / 60));
                    etaEl.textContent = `Llega aprox. en ${minutes} min`;
                }

                if (showMoto) {
                    window.namiMap._startMotoAnimation(state, coords);
                }
            })
            .catch(() => {
                // Sin conexión a OSRM: se conserva la línea recta de respaldo.
                if (etaEl) etaEl.textContent = 'No se pudo calcular la ruta por calles.';
                if (showMoto) window.namiMap._startMotoAnimation(state, [restLL, homeLL]);
            });
    },

    // Anima un marcador de moto recorriendo la polyline de la ruta a velocidad constante.
    // NOTA: es una simulación visual (interpola entre los puntos de la ruta calculada por
    // OSRM en un lapso fijo); el tracking GPS real del repartidor queda como trabajo futuro.
    _startMotoAnimation: function (state, coords) {
        if (!state || !coords || coords.length < 2) return;

        const motoIcon = window.L.divIcon({
            className: 'nami-moto-marker',
            html: '<div style="font-size:22px; line-height:1; filter:drop-shadow(0 2px 3px rgba(0,0,0,.45));">🛵</div>',
            iconSize: [26, 26],
            iconAnchor: [13, 13]
        });

        state.motoMarker = window.L.marker(coords[0], { icon: motoIcon, zIndexOffset: 1000 }).addTo(state.map);

        let i = 0;
        const totalSteps = coords.length;
        const stepMs = Math.max(120, Math.round(12000 / totalSteps)); // recorrido completo simulado en ~12s, en bucle

        state.motoTimer = setInterval(() => {
            i = (i + 1) % totalSteps;
            state.motoMarker.setLatLng(coords[i]);
        }, stepMs);
    },

    dispose: function (elementId) {
        const state = window.namiMap._maps[elementId];
        if (state) {
            if (state.motoTimer) clearInterval(state.motoTimer);
            if (state.map) state.map.remove();
            delete window.namiMap._maps[elementId];
        }
    }
};
