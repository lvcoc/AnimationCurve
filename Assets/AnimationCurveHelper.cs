using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationCurveHelper : MonoBehaviour
{
    public AnimationCurve positionCurve;

    private List<GameObject> goList = new List<GameObject>();   // 缓存gameobject列表
    private List<Vector2> goPos = new List<Vector2>();          // 缓存gameobject原始坐标

    private float midPosY = 0f;             // 中点的Y坐标
    private bool isInit = false;            // 是否初始化
    private bool debugMode = false;         // 是否开启调试log

    private void Start()
    {
        SetAnimationCurveData(transform.gameObject);
    }
    private void CustomizedDebug(string input)
    {
        if (debugMode)
        {
            Debug.Log(input);
        }
    }

    private void Update()
    {
        UpdategoList();
    }

    // 外部接口，本例中从Start中调用
    public void SetAnimationCurveData(GameObject midObj)
    {
        if (isInit) return;

        midPosY = midObj.transform.position.y;
        goList.Clear();
        for (int i = 0; i < transform.childCount; ++i)
        {
            goList.Add(transform.GetChild(i).gameObject);
        }

        InitPositionY();
        InitOriginalPos();
        isInit = true;
    }

    public void OpendebugMode(bool setting)
    {
        debugMode = setting;
    }

    private void InitOriginalPos()
    {
        goPos.Clear();
        for (int i = 0; i < goList.Count; ++i)
        {
            goPos.Add(goList[i].transform.position);
        }
    }

    private void InitPositionY()
    {
        RectTransform rectTrans = goList[0].transform.GetComponent<RectTransform>();        // 本例中排列的gameobject高度统一，取第一个gameobject的高度
        float height = rectTrans.rect.height;
        Vector3 vec3;
        for (int i = 0; i < goList.Count; ++i)
        {
            vec3 = goList[i].transform.position;
            goList[i].transform.position = new Vector2(vec3.x, vec3.y - i * (height / 100 - 20f));     // 偏移量，根据实际显示效果调整
        }
    }

    void UpdategoList()
    {
        for (int i = 0; i < goList.Count; ++i)
        {
            Transform trans = goList[i].transform;
            float value = Mathf.Abs(trans.position.y - midPosY);
            float x = GetPositionX(value/200);//按比例换算到1啊应该是
           
            trans.position = new Vector3(goPos[i].x + x - 0.6f, trans.position.y);     // 偏移量，根据实际显示效果调整

            CustomizedDebug("UpdategoList: " + value + ", trans.position.y:" + trans.position.y + ", trans.position.y: " + trans.position.y + ", midPosY: " + midPosY);
        }
    }

    float GetPositionX(float value)
    {
        float lerpIndex = Mathf.Lerp(1, 0, value / 1);          //这里也有问题，惊了            
        float x = positionCurve.Evaluate(lerpIndex );     // 利用animation curve进行映射
        Debug.Log(x);
        CustomizedDebug("GetPositionX lerpIndex: " + lerpIndex + ", x: " + x);
        return x*50;
    }
}
