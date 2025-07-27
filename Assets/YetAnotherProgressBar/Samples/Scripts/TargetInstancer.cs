using UnityEngine;
using System.Collections;

public class TargetInstancer : MonoBehaviour
{
    [SerializeField]
    private GameObject[] targetsProtos = null;
    private Vector3 min = new Vector3(-10, 3, 15);
    private Vector3 max = new Vector3(10, 15, 30);

    private static int layer;

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            InstantiateNewTarget();
        }
        StartCoroutine(CreateTargets());

        layer = LayerMask.NameToLayer("Ground");
    }

    private IEnumerator CreateTargets()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.3f);
            InstantiateNewTarget();
        }
    }

    public static int GroundLayer
    {
        get { return layer; }
    }

    private void InstantiateNewTarget()
    {
        Transform t = Instantiate(targetsProtos[UnityEngine.Random.Range(0, targetsProtos.Length)]).transform;
        t.position = new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));
    }
}
