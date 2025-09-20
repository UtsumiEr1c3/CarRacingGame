using UnityEngine;

public class TestPerlin : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private float a = 0.06f; // 间隙, 平滑度
    public bool isUsePerLin;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Vector3[] posArr = new Vector3[100];
        float ranX = Random.Range(1, 1000);
        float ranY = Random.Range(1, 1000);
        for (int i = 0; i < posArr.Length; i++)
        {
            if (isUsePerLin)
            {
                posArr[i] = new Vector3(i * 0.1f, Mathf.PerlinNoise(i * a + ranX, i * a + ranY), 0);
            }
            else
            {
                posArr[i] = new Vector3(i * 0.1f, Random.value, 0);
            }
        }
        lineRenderer.SetPositions(posArr);
    }
}
