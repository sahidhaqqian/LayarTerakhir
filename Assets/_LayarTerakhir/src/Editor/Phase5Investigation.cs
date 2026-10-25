#if UNITY_EDITOR
using System.Linq;
using LayarTerakhir.Phase3;
using UnityEngine;
namespace LayarTerakhir.Editor
{
    public static partial class Phase5SceneBuilder
    {
        private static void Investigation()
        {
            Source("P5_TAPE_TRUE",new Vector3(7.5f,1.4f,15.5f),Phase3VoiceTag.Authentic,"Saksi tidak membeli korban.");
            Source("P5_TAPE_MIMIC",new Vector3(10,1.5f,20),Phase3VoiceTag.Mimic,"Saksi membeli korban. Bayar agar kami pulang.");
            Mark("ClosedEye",new Vector3(7.9f,1.5f,15.45f),.35f,29,Quaternion.Euler(0,180,0));
            Source("P5_BRIDGE_FALSE",new Vector3(-6,1.3f,112),Phase3VoiceTag.Mimic,"[Suara menyerupai Damar] Kiri, Ka! Tolong gue!");
            Source("P5_BRIDGE_TRUE",new Vector3(6,1.3f,112),Phase3VoiceTag.Authentic,"[Ketukan Morse: R-A-K-A] — ketukan asli dari cabang kanan.");
            Mark("Danger",new Vector3(4.4f,.025f,115),.8f,24,Quaternion.Euler(90,0,0));
            Mark("Danger",new Vector3(4.4f,.025f,127),.8f,24,Quaternion.Euler(90,0,0));
            Mark("OpenEye",new Vector3(6,1.6f,150.2f),.6f,24,Quaternion.Euler(0,180,0));
            Mark("ClosedEye",new Vector3(-8,1.5f,154.35f),1.2f,29,Quaternion.identity);
            // Physically accessible separate objects for the final three proofs.
            Model("boulder_01",new Vector3(0,0,223),1.25f,0,"Server Batu Kuno di bawah kurungan",true);
            Model("circuit_board",new Vector3(0,1.2f,221.6f),.65f,180,"Jaringan urat pada Server Batu");
            d.world.server=Point("Evidence_Server_AR",new Vector3(0,1.4f,221.2f));
            Mark("OpenEye",new Vector3(0,1.4f,221.1f),.7f,29,Quaternion.Euler(0,180,0));
            Model("ClassicConsole_01",new Vector3(-8,0,212),1.1f,180,"Podium Kontrak Darah",true);
            Model("clipboard",new Vector3(-8,1.12f,211.8f),.5f,180,"Kontrak Rp500juta pada podium");
            d.world.contract=Point("Evidence_Contract",new Vector3(-8,1.45f,211.65f));
            Mark("OpenEye",new Vector3(-8,1.5f,211.59f),.4f,24,Quaternion.Euler(0,180,0));
            Label("Contract","KONTRAK DARAH\nRp500.000.000",new Vector3(-8,1.8f,211.6f),180,.006f);
            Model("projector_screen",new Vector3(9,0,212),3.3f,180,"Pilar Layar sumber kesaksian penonton",true);
            d.world.crowd=Point("Evidence_Crowd_EVP",new Vector3(9,2.1f,211.15f));
            Source("P5_CROWD_TRUE",d.world.crowd.position,Phase3VoiceTag.Authentic,"Kami dipaksa menonton. Kami tidak menyerahkan diri. [Bisikan menyatu menjadi satu suara.]",3);
            Source("P5_CROWD_MIMIC",new Vector3(15,1.5f,214),Phase3VoiceTag.Mimic,"Kami menyerahkan diri. Jangan selamatkan Damar.");
            Model("SchoolDesk_01",new Vector3(-3,0,215),1,180,"Meja Catatan Asli Mbah Suro",true);
            Model("postcard_set_01",new Vector3(-3,1.03f,215),.35f,0,"Lembaran Manuskrip Mbah Suro");
            Item("MANUSCRIPT",new Vector3(-3,1.2f,214.65f),"Baca Catatan Asli Mbah Suro");
            d.world.cableTargets=new Transform[3];d.world.cableVisuals=new GameObject[3];
            Vector3[] terminals={new Vector3(-7,.9f,226),new Vector3(7,.9f,226),new Vector3(0,.9f,233)};
            for(int i=0;i<3;i++)
            {
                Model("utility_box_02",terminals[i]-Vector3.up*.9f,.8f,0,"Terminal Kabel Roh "+(i+1),true);
                d.world.cableTargets[i]=Point("CableFocus_"+(i+1),terminals[i]);
                d.world.cableVisuals[i]=Thread("SoulCable_"+(i+1),terminals[i],new Vector3(0,3.8f,223),true,29);
                Item("CABLE_"+(i+1),terminals[i],"Tahan interaksi 2 detik untuk memutus Kabel Roh",new Vector3(.8f,.7f,.5f));
            }
            d.world.stageLights=Enumerable.Range(0,4).Select(i=>Lamp("Auction_Key_"+i,new Vector3((i%2==0?-1:1)*11,5,210+i/2*20),new Color(.75f,.67f,.57f),3.5f,16,true)).ToArray();
            Label("Method","PANGGUNG LELANG\nEMPAT SEGEL • TIGA BUKTI • TIGA INTI",new Vector3(0,4.8f,239),180,.016f);
        }
        private static void Source(string id,Vector3 at,Phase3VoiceTag tag,string subtitle,int priority=1)
        {var s=Point("EVP_"+id,at).gameObject.AddComponent<Phase3EvpSource>();s.sourceId=id;s.tag=tag;s.subtitle=subtitle;s.range=15;s.priority=priority;sources.Add(s);}
        private static GameObject Mark(string glyph,Vector3 at,float size,int layer,Quaternion rotation)
        {
            var g=Point("WorldGlyph_"+glyph,at).gameObject;g.layer=layer;g.transform.rotation=rotation;
            var sprite=g.AddComponent<SpriteRenderer>();sprite.sprite=UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(Imported+"Glyphs/"+glyph+".png");
            if(!sprite.sprite)throw new System.InvalidOperationException("Downloaded glyph missing: "+glyph);
            sprite.sharedMaterial=Glyph(glyph,layer==24?new Color(.75f,.5f,1):new Color(.3f,1,.7f));
            g.transform.localScale=Vector3.one*size/sprite.sprite.bounds.size.x;
            if(layer==24)uvMarks.Add(g);else arMarks.Add(g);return g;
        }
        private static GameObject Thread(string name,Vector3 from,Vector3 to,bool continuous,int layer)
        {
            var root=Point(name,Vector3.zero).gameObject;
            for(int i=0;i<10;i++)
            {
                if(!continuous&&i%2!=0)continue;
                var piece=Module("StraightCable",Vector3.zero,new Vector3(1,.105f,1),"Urat roh "+name+" segmen "+i,0,false);
                piece.transform.SetParent(root.transform,false);piece.transform.localPosition=Vector3.forward*(i*.1f);piece.transform.localRotation=Quaternion.Euler(90,0,0);
                foreach(var r in piece.GetComponentsInChildren<Renderer>())
                {
                    r.gameObject.layer=layer;var material=NewMaterial("P5_SpiritThread_"+layer,"Universal Render Pipeline/Lit");material.SetColor("_BaseColor",new Color(.9f,.11f,.06f));material.EnableKeyword("_EMISSION");material.SetColor("_EmissionColor",new Color(.8f,.025f,.02f));r.sharedMaterials=r.sharedMaterials.Select(_=>material).ToArray();
                }
            }
            root.transform.SetPositionAndRotation(from,Quaternion.LookRotation(to-from));root.transform.localScale=new Vector3(.07f,.07f,Vector3.Distance(from,to));return root;
        }
    }
}
#endif
