using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ViewData : MonoBehaviour
{
    [DllImport("mx_dll")]
    extern static int set_data(string data);

    // Start is called before the first frame update
    void Start()
    {
        var res = set_data("F://yy//unity (2)//unity//Data");
        Debug.Log( "result = " + res.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
