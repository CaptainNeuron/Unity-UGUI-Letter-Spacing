/// 这个组件需要和Text同时添加到一个GameObject上
/// 可以调整Text Spacing来调整字间距，仅支持从左上开始的模式
/// 可以支持长段文字自动换行
/// 注意：
/// 1、不可使用ContentSizeFitter，如需要高度自适应可以把Vertical Fit调为Preferred Size
/// 2、Customized Line Height 真实行高：通常为0就可以。如果输入其他数字则使用该数字作为每行的真实行高用于计算，如果为0则使用textHeightDict，是使用字体“FZLanTYJW_Zhong”算出来的所有字号下的真实行高dict

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Effects/TextSpacing")]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Text))]
    public class TextSpacing : BaseMeshEffect, ILayoutSelfController
    {
        #region 属性
        public float _textSpacing = 1f;
        
        private Dictionary<int, float> textHeightDict = new Dictionary<int, float>
        {
            {1, 1}, {2, 3}, {3, 4}, {4, 5}, {5, 5}, {6, 7}, {7, 8}, {8, 9}, {9, 10}, {10, 12}, {11, 13}, {12, 14},
            {13, 15}, {14, 17}, {15, 17}, {16, 18}, {17, 20}, {18, 21}, {19, 22}, {20, 23}, {21, 25}, {22, 26},
            {23, 27}, {24, 27}, {25, 29}, {26, 30}, {27, 31}, {28, 32}, {29, 34}, {30, 35}, {31, 36}, {32, 37},
            {33, 39}, {34, 39}, {35, 40}, {36, 42}, {37, 43}, {38, 44}, {39, 45}, {40, 47}, {41, 48}, {42, 49},
            {43, 49}, {44, 51}, {45, 52}, {46, 53}, {47, 54}, {48, 56}, {49, 57}, {50, 58}, {51, 60}, {52, 61},
            {53, 61}, {54, 62}, {55, 64}, {56, 65}, {57, 66}, {58, 67}, {59, 69}, {60, 70}, {61, 71}, {62, 71},
            {63, 73}, {64, 74}, {65, 75}, {66, 77}, {67, 78}, {68, 79}, {69, 80}, {70, 82}, {71, 83}, {72, 83},
            {73, 84}, {74, 86}, {75, 87}, {76, 88}, {77, 89}, {78, 91}, {79, 92}, {80, 93}, {81, 94}, {82, 95},
            {83, 96}, {84, 97}, {85, 99}, {86, 100}, {87, 101}, {88, 102}, {89, 104}, {90, 105}, {91, 105}, {92, 106},
            {93, 108}, {94, 109}, {95, 110}, {96, 111}, {97, 113}, {98, 114}, {99, 115}, {100, 116}, {101, 117},
            {102, 118}, {103, 119}, {104, 121}, {105, 122}, {106, 123}, {107, 124}, {108, 126}, {109, 127}, {110, 127},
            {111, 128}, {112, 130}, {113, 131}, {114, 132}, {115, 134}, {116, 135}, {117, 136}, {118, 137}, {119, 138},
            {120, 139}, {121, 140}, {122, 141}, {123, 143}, {124, 144}, {125, 145}, {126, 146}, {127, 148}, {128, 149},
            {129, 149}, {130, 151}, {131, 152}, {132, 153}, {133, 154}, {134, 156}, {135, 157}, {136, 158}, {137, 159},
            {138, 160}, {139, 161}, {140, 162}, {141, 163}, {142, 165}, {143, 166}, {144, 167}, {145, 169}, {146, 170},
            {147, 170}, {148, 171}, {149, 173}, {150, 174}, {151, 175}, {152, 176}, {153, 178}, {154, 179}, {155, 180},
            {156, 181}, {157, 182}, {158, 183}, {159, 184}, {160, 185}, {161, 187}, {162, 188}, {163, 189}, {164, 191},
            {165, 192}, {166, 192}, {167, 193}, {168, 195}, {169, 196}, {170, 197}, {171, 198}, {172, 200}, {173, 201},
            {174, 202}, {175, 203}, {176, 204}, {177, 205}, {178, 206}, {179, 208}, {180, 209}, {181, 210}, {182, 211},
            {183, 213}, {184, 214}, {185, 214}, {186, 215}, {187, 217}, {188, 218}, {189, 219}, {190, 220}, {191, 222},
            {192, 223}, {193, 224}, {194, 226}, {195, 226}, {196, 227}, {197, 228}, {198, 230}, {199, 231}, {200, 232},
            {201, 233}, {202, 235}, {203, 236}, {204, 236}, {205, 237}, {206, 239}, {207, 240}, {208, 241}, {209, 243},
            {210, 244}, {211, 245}, {212, 246}, {213, 248}, {214, 248}, {215, 249}, {216, 250}, {217, 252}, {218, 253},
            {219, 254}, {220, 255}, {221, 257}, {222, 258}, {223, 258}, {224, 259}, {225, 261}, {226, 262}, {227, 263},
            {228, 265}, {229, 266}, {230, 267}, {231, 268}, {232, 270}, {233, 270}, {234, 271}, {235, 272}, {236, 274},
            {237, 275}, {238, 276}, {239, 277}, {240, 279}, {241, 280}, {242, 280}, {243, 282}, {244, 283}, {245, 284},
            {246, 285}, {247, 287}, {248, 288}, {249, 289}, {250, 290}, {251, 292}, {252, 292}, {253, 293}, {254, 294},
            {255, 296}, {256, 297}, {257, 298}, {258, 300}, {259, 301}, {260, 302}, {261, 302}, {262, 304}, {263, 305},
            {264, 306}, {265, 307}, {266, 309}, {267, 310}, {268, 311}, {269, 312}, {270, 314}, {271, 314}, {272, 315},
            {273, 317}, {274, 318}, {275, 319}, {276, 320}, {277, 322}, {278, 323}, {279, 324}, {280, 324}, {281, 326},
            {282, 327}, {283, 328}, {284, 329}, {285, 331}, {286, 332}, {287, 333}, {288, 334}, {289, 336}, {290, 336},
            {291, 337}, {292, 339}, {293, 340}, {294, 341}, {295, 342}, {296, 344}, {297, 345}, {298, 346}, {299, 346},
            {300, 348}
        };
        [HideInInspector] public int contentAutoHeight = 0;

        [SerializeField] protected FitMode m_VerticalFit = FitMode.Unconstrained;

        public int customizedLineHeight = 0;
        #endregion
        
        
        //变换前：
        //  每个字的左右间距为字的宽度w，字的宽度=字号
        //  每个字的上下间距为“真实行高”，这个行高是一个稍微比字号大一点的数，取决于字号和字体。通过枚举的方法获得
        //  可以参考https://zhaoxin.pro/15918614844798.html
        //  变换前的字的左上角坐标x1，y1（第一个字就是0, 0）
        //变换后：
        //  每个字的左右间距为w+s
        //  上下间距不变
        //  变换h后的字的左上角坐标x2，y2（第一个字还是0, 0， 第二个字(w+s, 0）
        //  注意通常变换后会每行的字数会变少，需要处理换行的情况
        //  offset = (x2-x1, y2-y1)
        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
            {
                // Debug.Log("Modifying !IsActive()");
                return;
            }
            // Debug.Log("Modifying");
            List<UIVertex> vertexs = new List<UIVertex>();
            vh.GetUIVertexStream(vertexs);
            int indexCount = vh.currentIndexCount;
            UIVertex vt;
            var textComp = GetComponent<Text>();
            // 每行的字数 col1是无字间距时
            var col1 = (int) Math.Floor(GetComponent<RectTransform>().sizeDelta.x / (textComp.fontSize));
            var col2 = (int) Math.Floor(GetComponent<RectTransform>().sizeDelta.x / (textComp.fontSize + _textSpacing));
            if (col2 == 0) col2 = 1;
            // Debug.Log($"col1: {col1}, col2:{col2}");
            //这一步需要获得真实的height，如果没有指定customizedLineHeight，则从textHeightDict获取高度（textHeightDict是以FZLanTYJW_Zhong字体枚举出来的所有字号下的真实高度）
            
            var height = customizedLineHeight==0 ? 
                textHeightDict[textComp.fontSize] * textComp.lineSpacing :
                customizedLineHeight;


            float y1 = 0;
            float x1 = 0;
            float y2 = 0;
            float x2 = 0;
            Vector3 offset;
            var charCount = 2;
            for (int start = 6; start < indexCount; start += 6) //第一个字不用改变位置, 所以i从6开始
            {
                if (charCount % col1 == 1)
                {
                    y1 -= height; // 字往下走 所以y坐标变小
                    x1 = 0;
                }
                else
                {
                    x1 += textComp.fontSize;
                }

                if (charCount % col2 == 1)
                {
                    y2 -= height;
                    x2 = 0;
                }
                else
                {
                    x2 += textComp.fontSize + _textSpacing;
                }

                offset = new Vector3(x2 - x1, y2 - y1, 0);

                // var temp = "";
                for (int i = start; i < start + 6; i++)
                {
                    vt = vertexs[i];
                    vt.position += offset;
                    vertexs[i] = vt;
                    // if (i == start || i == start + 3)
                    //     temp += vt.position.x + "," + vt.position.y + "/  ";
                    //以下注意点与索引的对应关系
                    if (i % 6 <= 2)
                    {
                        vh.SetUIVertex(vt, (i / 6) * 4 + i % 6);
                    }

                    if (i % 6 == 4)
                    {
                        vh.SetUIVertex(vt, (i / 6) * 4 + i % 6 - 1);
                    }
                }

                charCount++;
                // Debug.Log(temp);
            }

            contentAutoHeight = (int) (-y2 + height);
            SetDirty();
            // if (_verticalAutoSizeFit)
            // {
            //     var sd = GetComponent<RectTransform>().sizeDelta;
            //     GetComponent<RectTransform>().sizeDelta = new Vector2(sd.x, -x2 + height);
            // }

        }
        ///// 以下代码参考ContentSizeFitter
        
        /// <summary>
        /// The size fit modes available to use.
        /// </summary>
        public enum FitMode
        {
            /// <summary>
            /// Don't perform any resizing.
            /// </summary>
            Unconstrained,
            /// <summary>
            /// Resize to the preferred size of the content.
            /// </summary>
            PreferredSize
        }
        /// <summary>
        /// The fit mode to use to determine the width.
        /// </summary>
        /// <summary>
        /// The fit mode to use to determine the height.
        /// </summary>
        public FitMode verticalFit { get { return m_VerticalFit; }
            set
            {
                if (value != m_VerticalFit)
                {
                    m_VerticalFit = value;
                    SetDirty();    
                }
            } }

        [System.NonSerialized] private RectTransform m_Rect;
        private RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                    m_Rect = GetComponent<RectTransform>();
                return m_Rect;
            }
        }

        private DrivenRectTransformTracker m_Tracker;

        protected override void OnEnable()
        {
            base.OnEnable();
            SetDirty();
        }

        protected override void OnDisable()
        {
            m_Tracker.Clear();
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            base.OnDisable();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            SetDirty();
        }

        private void HandleSelfFittingAlongAxis()
        {
            if (verticalFit == FitMode.Unconstrained)
            {
                // Keep a reference to the tracked transform, but don't control its properties:
                m_Tracker.Add(this, rectTransform, DrivenTransformProperties.None);
                return;
            }

            m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaY);

            // Set size to preferred size
            rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)1, contentAutoHeight);
        }

        /// <summary>
        /// Calculate and apply the horizontal component of the size to the RectTransform
        /// </summary>
        public virtual void SetLayoutHorizontal()
        {
            m_Tracker.Clear();
            HandleSelfFittingAlongAxis();
        }

        /// <summary>
        /// Calculate and apply the vertical component of the size to the RectTransform
        /// </summary>
        public virtual void SetLayoutVertical()
        {
            HandleSelfFittingAlongAxis();
        }

        protected void SetDirty()
        {
            if (!IsActive())
                return;

            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

    #if UNITY_EDITOR
        protected override void OnValidate()
        {
            SetDirty();
        }

    #endif
    }
}