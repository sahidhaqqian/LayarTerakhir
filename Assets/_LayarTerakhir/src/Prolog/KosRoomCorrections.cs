using LayarTerakhir.Core;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.UI;

namespace LayarTerakhir.Prolog
{
    // Targeted, additive corrections requested after approval; never rebuild the Golden room.
    public static class KosRoomCorrections
    {
        public static void Apply(KamarKosPrologDirector director)
        {
            UiWidgets.Font=PresentationProfile.Load()?.bodyFont;
            var jacket=director.jacketItem;
            if(jacket)
            {
                var visible=jacket.transform.Find("HangingJacket");
                if(visible)
                {
                    jacket.transform.position+=new Vector3(1.40f,1.34f,-1.68f)-BoundsOf(visible.gameObject).center;
                    foreach(var r in visible.GetComponentsInChildren<Renderer>())
                    {
                        r.enabled=true;var materials=r.sharedMaterials;
                        for(int i=0;i<materials.Length;i++)if(materials[i].name.Contains("Jacket")){materials[i]=new Material(materials[i]);materials[i].SetColor("_BaseColor",new Color(.72f,.78f,.62f));}
                        r.sharedMaterials=materials;
                    }
                    var collider=jacket.GetComponent<BoxCollider>();var b=BoundsOf(visible.gameObject);
                    collider.center=jacket.transform.InverseTransformPoint(b.center);
                    collider.size=new Vector3(b.size.x/jacket.transform.lossyScale.x,b.size.y/jacket.transform.lossyScale.y,.15f/jacket.transform.lossyScale.z);
                    jacket.promptLabel="Pakai jaket";
                    var hook=GameObject.Find("JacketHook");
                    if(hook)
                    {
                        var rail=new GameObject("ReachableJacketHook").AddComponent<LineRenderer>();rail.transform.SetParent(director.transform,true);rail.useWorldSpace=true;rail.positionCount=3;rail.startWidth=rail.endWidth=.008f;rail.sharedMaterial=hook.GetComponentInChildren<Renderer>().sharedMaterial;
                        rail.SetPositions(new[]{new Vector3(1.40f,1.68f,-1.91f),new Vector3(1.40f,1.68f,-1.68f),new Vector3(1.40f,1.66f,-1.68f)});hook.SetActive(false);
                    }
                }
            }
            var desk=GameObject.Find("wooden_table_02");
            if(desk)
            {
                var b=BoundsOf(desk);var key=new GameObject("MotorKeyOnDesk");
                key.transform.position=new Vector3(b.center.x-.40f,b.max.y+.022f,b.min.z+.16f);
                key.transform.SetParent(director.transform,true);
                var item=key.AddComponent<KosInteractableItem>();item.director=director;item.itemId="KUNCI_MOTOR_MEJA";item.promptLabel="Ambil kunci motor";
                var hit=key.AddComponent<BoxCollider>();hit.size=new Vector3(.15f,.045f,.11f);
                var metal=new Material(Shader.Find("Universal Render Pipeline/Lit"));metal.SetColor("_BaseColor",new Color(.58f,.57f,.50f));metal.SetFloat("_Metallic",.8f);
                var rubber=new Material(metal);rubber.SetColor("_BaseColor",new Color(.055f,.06f,.05f));rubber.SetFloat("_Metallic",0);
                Part("RubberFob",key.transform,new Vector3(-.025f,.005f,0),new Vector3(.044f,.018f,.055f),rubber);
                Part("KeyBlade",key.transform,new Vector3(.025f,.005f,0),new Vector3(.065f,.008f,.012f),metal);
                for(int i=0;i<3;i++)Part("KeyTooth",key.transform,new Vector3(.022f+i*.012f,.005f,-.009f),new Vector3(.006f,.008f,.01f),metal);
                var ring=new GameObject("KeyRing").AddComponent<LineRenderer>();ring.transform.SetParent(key.transform,false);ring.useWorldSpace=false;ring.loop=true;ring.positionCount=20;ring.startWidth=ring.endWidth=.002f;ring.sharedMaterial=metal;
                for(int i=0;i<20;i++){float a=i*Mathf.PI*.1f;ring.SetPosition(i,new Vector3(-.061f+Mathf.Cos(a)*.018f,.005f,Mathf.Sin(a)*.018f));}
                director.motorKeyItem=item;
            }
            AlignMonitor();
            ReplaceClue("StickyNote",false);ReplaceClue("CalendarAugust",true);
            ReplacePadlock();
        }
        public static Bounds BoundsOf(GameObject go)
        {
            var result=new Bounds(go.transform.position,Vector3.zero);bool first=true;
            foreach(var r in go.GetComponentsInChildren<Renderer>())if(r.enabled){if(first){result=r.bounds;first=false;}else result.Encapsulate(r.bounds);}
            return result;
        }
        private static void Part(string name,Transform root,Vector3 p,Vector3 scale,Material material)
        {var go=GameObject.CreatePrimitive(PrimitiveType.Cube);go.name=name;go.transform.SetParent(root,false);go.transform.localPosition=p;go.transform.localScale=scale;go.GetComponent<Renderer>().sharedMaterial=material;Object.Destroy(go.GetComponent<Collider>());}
        public static void AlignMonitor()
        {
            var monitor=GameObject.Find("computerScreen");var panel=GameObject.Find("MonitorDashboard");if(!monitor||!panel)return;
            var b=BoundsOf(monitor);
            // Surveyed actual front-glass vertices (not the stand AABB): top y=1.2038,
            // z=1.4236; bottom y=.8500,z=1.3766. Glass normal=(0,.1314,-.9913).
            // Follow its 7.55-degree tilt, with only 1mm separation to avoid z-fighting.
            var rotation=Quaternion.Euler(7.55f,0,0);var normal=rotation*Vector3.back;
            var target=new Vector3(b.center.x,b.min.y+b.size.y*.582f,b.center.z)+normal*.001f;
            panel.transform.SetPositionAndRotation(target,rotation);
            var scale=panel.transform.lossyScale;var local=panel.transform.localScale;
            panel.transform.localScale=new Vector3(local.x*(b.size.x*.865f)/scale.x,local.y*(b.size.y*.62f)/scale.y,local.z);
        }
        private static void ReplacePadlock()
        {
            var original=GameObject.Find("CombinationPadlock");if(!original)return;var b=BoundsOf(original);
            foreach(var renderer in original.GetComponentsInChildren<Renderer>())renderer.enabled=false;
            var root=new GameObject("FourWheelDrawerLock").transform;root.SetParent(original.transform.parent,true);root.position=b.center;
            var metal=new Material(Shader.Find("Universal Render Pipeline/Lit"));metal.SetColor("_BaseColor",new Color(.34f,.26f,.12f));metal.SetFloat("_Metallic",.7f);
            var dark=new Material(metal);dark.SetColor("_BaseColor",new Color(.08f,.085f,.075f));
            Part("LockBody",root,new Vector3(0,.005f,0),new Vector3(.10f,.065f,.034f),metal);
            for(int i=0;i<4;i++)
            {
                var wheel=GameObject.CreatePrimitive(PrimitiveType.Cylinder);wheel.name="PhysicalDigitWheel_"+i;wheel.transform.SetParent(root,false);wheel.transform.localPosition=new Vector3((i-1.5f)*.022f,-.013f,-.021f);wheel.transform.localRotation=Quaternion.Euler(0,0,90);wheel.transform.localScale=new Vector3(.027f,.008f,.027f);wheel.GetComponent<Renderer>().sharedMaterial=dark;Object.Destroy(wheel.GetComponent<Collider>());
            }
            var shackle=new GameObject("LockShackle").AddComponent<LineRenderer>();shackle.transform.SetParent(root,false);shackle.useWorldSpace=false;shackle.positionCount=17;shackle.startWidth=shackle.endWidth=.006f;shackle.sharedMaterial=metal;
            for(int i=0;i<17;i++){float angle=i*Mathf.PI/16;shackle.SetPosition(i,new Vector3(Mathf.Cos(angle)*.029f,.034f+Mathf.Sin(angle)*.035f,0));}
        }
        private static void ReplaceClue(string name,bool calendar)
        {
            var surface=GameObject.Find(name);if(!surface)return;var renderer=surface.GetComponent<Renderer>();if(!renderer)return;
            var material=new Material(renderer.sharedMaterial);material.SetTexture("_BaseMap",null);material.SetColor("_BaseColor",calendar?new Color(.68f,.64f,.49f):new Color(.68f,.59f,.22f));
            material.SetColor("_EmissionColor",Color.black);renderer.sharedMaterial=material;
            var go=new GameObject(name+"LegibleClue",typeof(RectTransform),typeof(Canvas));var canvas=go.GetComponent<Canvas>();canvas.renderMode=RenderMode.WorldSpace;
            var rect=go.GetComponent<RectTransform>();rect.sizeDelta=new Vector2(512,calendar?700:512);go.transform.SetPositionAndRotation(surface.transform.position-surface.transform.forward*.0015f,surface.transform.rotation);
            go.transform.localScale=new Vector3(surface.transform.lossyScale.x/512,surface.transform.lossyScale.y/(calendar?700:512),.001f);go.transform.SetParent(surface.transform,true);
            Color ink=new Color(.18f,.16f,.12f);
            void Words(string id,string words,Vector2 min,Vector2 max,int size){var t=UiWidgets.Text(id,go.transform,words,min,max,size,TextAnchor.UpperLeft);t.color=ink;}
            if(!calendar){Words("PuzzleNote","Empat putaran.\nYang merawatku\nlebih dulu.\n\nHari mendahului\nbulan.",new Vector2(.07f,.06f),new Vector2(.95f,.93f),38);return;}
            Words("Month","AGUSTUS",new Vector2(.07f,.82f),new Vector2(.95f,.96f),56);
            Words("WeekDays","S  S  R  K  J  S  M",new Vector2(.07f,.73f),new Vector2(.95f,.81f),29);
            for(int day=1;day<=31;day++)
            {
                int n=day-1;float x=.075f+n%7*.13f,y=.65f-n/7*.085f;
                Words("Day"+day,day.ToString(),new Vector2(x,y),new Vector2(x+.12f,y+.08f),34);
            }
            Words("BirthdayClue","17 • Merdeka\nIbu bertambah usia\ntiga hari sebelumnya.\nJangan lupa telepon.",new Vector2(.07f,.025f),new Vector2(.95f,.29f),31);
        }
    }
}
