using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deform;
using DG.Tweening;
using Sirenix.OdinInspector;

public class AITree : MonoBehaviour
{

    public GameObject jumpPoint;

    [HideInInspector]
    public BendDeformer bendDeformer;
    [HideInInspector]
    public bool isBending = false;

    //private GameObject player;
    private System.Random random = new System.Random();

    [Button(ButtonSizes.Small)]
    private void TestBend()
    {
        //TweenBendDeformer(1, Quaternion.Euler(new Vector3(0, random.Next(0, 361), 0)));
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the jump point of this tree
        if(jumpPoint == null)
        {
            Debug.LogError($"{this.gameObject.name} does not have a jump point Assigned!", this.gameObject);
        }

        // Get the deformer object(S)
        bendDeformer = this.gameObject.GetComponentInChildren<BendDeformer>();

        // Instantiate player object for testing bend
        //player = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TweenBendDeformer(float _speed, Quaternion _rotation)
    {
        Quaternion _bendDeformerRotation = Quaternion.Euler(new Vector3(0, _rotation.eulerAngles.y -90, 0));
        isBending = true;
        bendDeformer.gameObject.transform.rotation = _bendDeformerRotation;
        DOTween.To(()=> bendDeformer.Angle, x=> bendDeformer.Angle = x, 30, 1).OnComplete(()=>DOTween.To(()=> bendDeformer.Angle, x=> bendDeformer.Angle = x, 0, 0.5f).OnComplete(()=>{isBending =  false;}));
    }
}
