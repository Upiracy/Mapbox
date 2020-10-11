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
        Action<List<Vector3>> callback;

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

        public void FindPlayer(Vector3 destination)
        {
           // Debug.Log(transform.localPosition);
            startPoint.position = transform.localPosition;
            endPoint.position = destination;
            Query(GetPositions, startPoint, endPoint, map);
        }

        void GetPositions(List<Vector3> vecs)
        {
            UnityEngine.Debug.Log("GetPositions");
            foreach (var i in vecs)
            {
                UnityEngine.Debug.Log(i);
                GameObject point = Instantiate<GameObject>((GameObject)Resources.Load("Point"), transform.position, transform.rotation);
                point.transform.position = i;
            }
        }

        private void Query(Action<List<Vector3>> vecs, Transform start, Transform end, AbstractMap map)
        {
            if (callback == null)
                callback = vecs;

            Vector2d[] wp = new Vector2d[2];
            wp[0] = start.GetGeoPosition(map.CenterMercator, map.WorldRelativeScale);
            wp[1] = end.GetGeoPosition(map.CenterMercator, map.WorldRelativeScale);
            DirectionResource _directionResource = new DirectionResource(wp, RoutingProfile.Walking);
            _directionResource.Steps = true;

            _directions.Query(_directionResource, HandleDirectionsResponse);
        }

        void HandleDirectionsResponse(DirectionsResponse response)
        {
            if (null == response.Routes || response.Routes.Count < 1)
            {
                return;
            }
       
            var dat = new List<Vector3>();
            foreach (var point in response.Routes[0].Geometry)
            {
                dat.Add(Conversions.GeoToWorldPosition(point.x, point.y, map.CenterMercator, map.WorldRelativeScale).ToVector3xz());
            }
            callback(dat);
        }
    }
}