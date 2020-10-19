using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Utils;
using Mapbox.Unity.Utilities;
using Mapbox.Unity;
using System;
using Mapbox.Directions;
using System.Diagnostics;

namespace Mapbox.Examples
{
    public class FindPath : MonoBehaviour
    {
        [SerializeField]
        AbstractMap map;
        Directions.Directions _directions;

        [SerializeField]
        Transform startPoint;
        [SerializeField]
        Transform endPoint;

        void Awake()
        {
            _directions = MapboxAccess.Instance.Directions;


        }

        IEnumerator Start()
        {
            yield return new WaitForSeconds(1);

            // FindPlayer(new Vector3(80, 0, -50));
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void FindTarget(Vector3 startPos, Vector3 destination, Action<List<Vector3>> callback)
        {
            // Debug.Log(transform.localPosition);
            startPoint.position = startPos;
            endPoint.position = destination;
            ResponseHandler handler = new ResponseHandler(callback, map);
            Query(callback, startPoint, endPoint, map, handler);
        }

        private void Query(Action<List<Vector3>> vecs, Transform start, Transform end, AbstractMap map, ResponseHandler handler)
        {
            Vector2d[] wp = new Vector2d[2];
            wp[0] = start.GetGeoPosition(map.CenterMercator, map.WorldRelativeScale);
            wp[1] = end.GetGeoPosition(map.CenterMercator, map.WorldRelativeScale);
            DirectionResource _directionResource = new DirectionResource(wp, RoutingProfile.Walking);
            _directionResource.Steps = true;

            _directions.Query(_directionResource, handler.HandleDirectionsResponse);
        }


    }

    class ResponseHandler
    {
        public Action<List<Vector3>> callback;
        AbstractMap map;

        public ResponseHandler(Action<List<Vector3>> callback, AbstractMap map)
        {
            this.map = map;
            this.callback = callback;
        }

        public void HandleDirectionsResponse(DirectionsResponse response)
        {
            if (null == response.Routes || response.Routes.Count < 1)
            {
                return;
            }

            List<Vector3> dat = new List<Vector3>();
            foreach (var point in response.Routes[0].Geometry)
            {
                dat.Add(Conversions.GeoToWorldPosition(point.x, point.y, map.CenterMercator, map.WorldRelativeScale).ToVector3xz());
            }
            callback(dat);
        }
    }
}

