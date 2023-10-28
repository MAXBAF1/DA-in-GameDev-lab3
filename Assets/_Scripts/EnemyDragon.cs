using UnityEngine;
using UnityEngine.Serialization;

public class EnemyDragon : MonoBehaviour
{
    [SerializeField] private GSParser parser;
    [SerializeField] private int level;

    public GameObject dragonEggPrefab;
    public float speed;
    public float leftRightDistance;
    public float timeBetweenEggDrops;
    public float chanceDirection;

    private void Start()
    {
        Invoke(nameof(Initialize), 1f);
        Invoke(nameof(DropEgg), 2f);
    }

    private void Update()
    {
        var pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        if (pos.x < -leftRightDistance) speed = Mathf.Abs(speed);
        else if (pos.x > leftRightDistance) speed = -Mathf.Abs(speed);
    }

    private void DropEgg()
    {
        var myVector = new Vector3(0.0f, 5.0f, 0.0f);
        var egg = Instantiate(dragonEggPrefab);
        egg.transform.position = transform.position + myVector;
        Invoke(nameof(DropEgg), timeBetweenEggDrops);
    }

    private void FixedUpdate() 
    {
        if (Random.value < chanceDirection) speed *= -1;
    }

    private void Initialize()
    {
        speed = parser.Data[level][0];
		timeBetweenEggDrops = parser.Data[level][1];
        leftRightDistance = parser.Data[level][2];
        chanceDirection = 0.01f;
    }
}