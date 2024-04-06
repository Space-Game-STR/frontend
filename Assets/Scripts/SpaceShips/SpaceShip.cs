using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    public SpaceShipClass spaceShip;

    private bool pathMade;
    private JourneyClass journey;
    public LineRenderer lr;
    private Celestial celestialStart;
    private Celestial celestialEnd;

    public long startTime;
    public long endTime;

    void Start()
    {
        InvokeRepeating("SetCurrentPosFalse", 5f, 5f);
    }

    void Update()
    {
        if (CelestialsManager.instance.celestials.Count == 0) return; //Mientras no haya planetas, no hacer nada
        if (!pathMade)
        {
            JourneyClass res = JourneysManager.instance.SearchJourneyByShip(spaceShip.uuid);
            if (res != null)
            {
                journey = res;

                celestialStart = CelestialsManager.instance.GetCelestial(journey.celestialStart);
                celestialEnd = CelestialsManager.instance.GetCelestial(journey.celestialEnd);

                startTime = journey.start;
                endTime = journey.end;

                SetLine();
                SetCurrentPos(true);
                pathMade = true;
            }
        }
    }

    public void SetClass(SpaceShipClass spaceShipClass)
    {
        spaceShip = spaceShipClass;
    }

    public void SetLine()
    {
        Vector2 startPoint = celestialStart.transform.position;
        Vector2 endPoint = celestialEnd.transform.position;

        lr.positionCount = 2;

        lr.SetPosition(0, startPoint);
        lr.SetPosition(1, endPoint);
    }

    private void SetCurrentPosFalse() {
        SetCurrentPos(false);
    }

    public void SetCurrentPos(bool instant)
    {
        long currentTimeMillis = (long)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds;
        float t = CalculateInterpolationFactor(startTime, endTime, currentTimeMillis);

        Vector3 currentPosition = Vector3.Lerp(celestialStart.transform.position, celestialEnd.transform.position, t);
        currentPosition.z = -1;

        if (t == 1)
        {
            //If the ship arrives, we search again search for the new ships
            SpaceshipsManager.instance.GetSpaceShips();
        }
        if (instant)
        {
            transform.position = currentPosition;
        }
        else
        {
            StartCoroutine(Move(transform.position, currentPosition, 5));
        }
    }

    IEnumerator Move(Vector3 beginPos, Vector3 endPos, float time)
    {
        for (float t = 0; t < 1; t += Time.deltaTime / time)
        {
            transform.position = Vector3.Lerp(beginPos, endPos, t);
            yield return null;
        }
    }

    float CalculateInterpolationFactor(long start, long end, long current)
    {
        current = Clamp(current, start, end);

        long totalDuration = end - start;

        long currentDuration = current - start;

        float t = (float)currentDuration / totalDuration;

        return t;
    }

    public long Clamp(long value, long min, long max)
    {
        if (value < min)
            return min;
        else if (value > max)
            return max;
        else
            return value;
    }
}
