using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObj : MonoBehaviour
{
    public Dictionary<string, int> doorPatten = new Dictionary<string, int>
    {
        // One
        {  "L",     0       },
        {  "T",     90      },
        {  "R",     180     },
        {  "B",     270     },

        // Curve 기억자 형태
        {  "LB",    0       },
        {  "LT",    90      },
        {  "TR",    180     },
        {  "RB",    270     },

        // Straight 일자 형태
        {  "LR",     0       },
        {  "TB",     90      },

        // Triple 일자 형태
        {  "LTB",     0       },
        {  "LTR",     90      },
        {  "TRB",     180      },
        {  "LRB",     270      },

        
        // quad 일자 형태
        {  "LTRB",     0       }
    };

    public Dictionary<string, string> roomPatten = new Dictionary<string, string>
    {
        // One
        {  "L",     "None"       },
        {  "T",     "None"       },
        {  "R",     "None"       },
        {  "B",     "None"       },

        // Curve 기억자 형태
        {  "LB",    "Curve"      },
        {  "LT",    "Curve"      },
        {  "TR",    "Curve"      },
        {  "RB",    "Curve"      },


        // Straight 일자 형태
        {  "LR",     "Straight"  },
        {  "TB",     "Straight"  },


        // Triple 일자 형태(RLT)
        {  "LTB",     "Triple"   },
        {  "LTR",     "Triple"   },
        {  "TRB",     "Triple"   },
        {  "LRB",     "Triple"   },

        // quad 일자 형태
        {  "LTRB",     "Quad"    }
    };
    public Transform objTrasform { get; set; }
    public string roomType { get; set; }
    public string objParren { get; set; }
    public int  objIndex { get; set; }




}
