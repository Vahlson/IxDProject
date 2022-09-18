using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _platforms;
    private int _zOffset;
    private int _xOffset;
    private string[] _directions = { "Left", "Right", "Forward" };
    // Start is called before the first frame update
    GameObject passed;
    GameObject player;
    animationStateController p;

    void Start()
    {
        p = GameObject.FindObjectOfType<animationStateController>();
        player=p.gameObject;
        //passed = gameobject.find start.
        for (int i = 0; i < _platforms.Length; i++)
        {
            GameObject g = Instantiate(_platforms[i], transform.position, Quaternion.Euler(0, 0, 0));
            g.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            test(g.GetComponentInChildren<DirectionHitBox>().gameObject);
        }

    }


    public void RecyclePlatform(GameObject platform)
    {

        if (passed != null)
        {
            passed.transform.position = transform.position;
            test(passed.GetComponentInChildren<DirectionHitBox>().gameObject);
        }
        passed = platform;

    }
    private void test(GameObject gameobject)
    {
        gameobject.tag = _directions[Random.Range(0, _directions.Length)];
        Quaternion tempRotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
         transform.rotation.eulerAngles.y,
          transform.rotation.eulerAngles.z);
        Vector3 tempPosition = transform.position;


        switch (gameobject.tag)
        {
            case "Left":
                transform.Rotate(new Vector3(0, -90, 0));
                // replace with object size.
                transform.position += transform.forward * 10;
                break;
            case "Right":
                transform.Rotate(new Vector3(0, 90, 0));
                transform.position += transform.forward * 10;
                break;
            case "Forward":
                transform.Rotate(new Vector3(0, 0, 0));
                transform.position += transform.forward * 10;
                break;

        }
        foreach (var item in _platforms)
        {
            if (item.transform.position == this.transform.position)
            {
                this.transform.rotation = tempRotation;
                this.transform.position = tempPosition;
                test(gameobject);
                return;
            }
        }
        print("next pos will be:" + this.transform);
        print(gameObject.transform);
    }
}
