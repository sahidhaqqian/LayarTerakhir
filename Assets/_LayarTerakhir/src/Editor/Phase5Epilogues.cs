#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LayarTerakhir.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Object = UnityEngine.Object;
namespace LayarTerakhir.Editor
{
    public static partial class Phase5SceneBuilder
    {
        private static void Epilogues()
        {
            d.world.epilogueRoot=Point("Final_Epilogue_New_Sets",Vector3.zero).gameObject;
            d.world.epilogueSets=new GameObject[3];
            string[] names={"Bad_Studio_Satu_Bulan","Dawn_Atmaja_Exterior","Secret_Empty_Market"};
            Vector3[] centers={new Vector3(100,0,220),new Vector3(140,0,220),new Vector3(185,0,220)};
            for(int i=0;i<3;i++)d.world.epilogueSets[i]=Point(names[i],centers[i],d.world.epilogueRoot.transform).gameObject;
            // Parent only the new objects placed for each epilogue. The existing
            // Kos/Atmaja environment prefabs are never copied into these sets.
            int before=env.childCount;
            Floor("StudioFloor",centers[0],new Vector2(11,12),"concrete_wall_007");
            Wall(centers[0]+new Vector3(0,0,6),new Vector2(11,3.3f),"Studio background");Wall(centers[0]+new Vector3(-5.5f,0,0),new Vector2(12,3.3f),"Studio side wall",90);
            Wall(centers[0]+new Vector3(5.5f,0,0),new Vector2(12,3.3f),"Studio right wall",90);
            Wall(centers[0]+new Vector3(0,0,-6),new Vector2(11,3.3f),"Studio entry wall");
            Floor("StudioCeiling",centers[0]+Vector3.up*3.5f,new Vector2(11,12),"concrete_wall_007");
            Model("ClassicConsole_01",centers[0]+new Vector3(0,0,3.5f),.95f,180,"Meja studio Raka pada Bad Ending",true);
            Model("classic_laptop",centers[0]+new Vector3(0,.97f,3.5f),.5f,180,"Dashboard naratif Bad Ending");
            Model("Camera_01",centers[0]+new Vector3(1.2f,.97f,3.5f),.35f,180,"Perlengkapan kanal Tuan Rumah Baru");
            Model("BarberShopChair_01",centers[0]+new Vector3(0,0,2.3f),1.25f,180,"Kursi studio Tuan Rumah Baru");
            Model("AntiqueMirror",centers[0]+new Vector3(3,0,4),2.6f,180,"Cermin studio akhir buruk");
            Label("BankBad","OPERASI IBU LUNAS\nSALDO Rp500.000.000\nAKUN: Penonton_Terakhir",centers[0]+new Vector3(0,1.85f,3.3f),180,.0065f);
            Lamp("StudioRed",centers[0]+new Vector3(1,2.6f,2),new Color(.9f,.08f,.06f),5,9);
            ParentNew(before,d.world.epilogueSets[0].transform);
            before=env.childCount;
            Floor("DawnWetRoad",centers[1],new Vector2(70,76),"brown_mud_03");
            Model("AtmajaEpilogueHouse",centers[1]+new Vector3(0,-.25f,13),7,180,"Fasad Atmaja baru untuk epilog subuh",true);
            AnimatedHuman("DamarFinale",centers[1]+new Vector3(.8f,0,1.6f),1.75f,180,"Damar kembali di depan Rumah Atmaja");
            Model("AntiqueMirror",centers[1]+new Vector3(-2,0,2.5f),2,180,"Pantulan epilog Siaran Terputus");
            Label("DawnTime","05:30 WIB",centers[1]+new Vector3(0,3,6),180,.01f);
            Lamp("DawnSoft",centers[1]+new Vector3(0,4,1),new Color(.75f,.84f,.95f),4,17);
            ParentNew(before,d.world.epilogueSets[1].transform);
            before=env.childCount;
            Floor("MarketAfterRelease",centers[2],new Vector2(30,28),"brown_mud_03");
            Model("MarketStall",centers[2]+new Vector3(-9,0,8),3.4f,90,"Tenda kosong setelah kesaksian");
            Model("MarketStall",centers[2]+new Vector3(9,0,8),3.4f,-90,"Tenda kosong setelah pasar runtuh");
            Model("ArmChair_01",centers[2]+new Vector3(0,0,5),1.1f,180,"Kursi penonton kosong, shot terakhir Secret");
            Model("AntiqueMirror",centers[2]+new Vector3(2,0,6),2.5f,180,"Cermin biasa tanpa wajah tersisa");
            AnimatedHuman("DamarFinale",centers[2]+new Vector3(.8f,0,1.6f),1.75f,180,"Damar di epilog Saksi Tanpa Layar");
            for(int i=0;i<9;i++)
            {
                var witness=AnimatedHuman("MemoryGhost",centers[2]+new Vector3((i%3-1)*3,0,12+(i/3)*3),i<3?1.6f:1.1f,0,"Siluet kesaksian korban lama "+i);
                foreach(var r in witness.GetComponentsInChildren<Renderer>())r.shadowCastingMode=ShadowCastingMode.Off;
            }
            Lamp("WitnessDawn",centers[2]+new Vector3(0,5,5),new Color(.85f,.83f,.68f),5,26);
            ParentNew(before,d.world.epilogueSets[2].transform);
            var shots=new List<LayarTerakhir.Phase5.Phase5EndingShot>();
            void Shot(EndingId id,Vector3 from,Vector3 to,Vector3 look,string speaker,string text,float duration,int set=-1)
            {shots.Add(new LayarTerakhir.Phase5.Phase5EndingShot{ending=id,from=Point("Shot_"+id+"_"+shots.Count+"_From",from),to=Point("Shot_"+id+"_"+shots.Count+"_To",to),look=Point("Shot_"+id+"_"+shots.Count+"_Look",look),speaker=speaker,subtitle=text,seconds=duration,epilogueSet=set});}
            Shot(EndingId.BAD,new Vector3(-2,1.7f,216),new Vector3(-1.2f,1.5f,218),new Vector3(0,1,220),"Damar","Ka... lo beneran milih itu? [Tangan pucat menarik Damar ke balik lantai cermin.]",8);
            Shot(EndingId.BAD,centers[0]+new Vector3(-3,1.8f,-2),centers[0]+new Vector3(-2,1.7f,0),centers[0]+new Vector3(0,1.3f,3.3f),"SATU BULAN KEMUDIAN","Operasi Ibu lunas. Rp500.000.000 tertera di dashboard. Di studio, Raka menulis dengan akun baru: Penonton_Terakhir.",11,0);
            Shot(EndingId.BAD,centers[0]+new Vector3(4,1.5f,.5f),centers[0]+new Vector3(3.5f,1.5f,1),centers[0]+new Vector3(3,1.4f,4),"Penonton_Terakhir","[Di cermin studio, Damar berdiri di belakang sang tuan rumah baru.] Siaran berikutnya akan segera dimulai.",9,0);
            Shot(EndingId.NORMAL,new Vector3(-14,1.8f,204),new Vector3(-12.5f,1.6f,205),new Vector3(-16,1.2f,207),"Raka","Gue udah janji bakal bawa lo pulang, Dam. Maafin gue kalau gue nggak bisa ikut.",9);
            Shot(EndingId.NORMAL,new Vector3(11,2,216),new Vector3(13,1.8f,217),new Vector3(16,1.5f,220),"Siaran Terputus","[HP Raka pecah. Jaringan siaran padam; Damar didorong melewati cermin sebelum Pasar Setan runtuh.]",8);
            Shot(EndingId.NORMAL,centers[1]+new Vector3(-3,1.8f,-3),centers[1]+new Vector3(-2,1.6f,-1),centers[1]+new Vector3(.7f,1.2f,1.6f),"Damar","Satu juta penonton sadar. Raka Wijaya masih dinyatakan hilang. [Di kaca HP yang mati, pantulan Raka melambaikan tangan.]",11,1);
            Shot(EndingId.TRUE,new Vector3(18,1.8f,215),new Vector3(17,1.7f,217),new Vector3(13,1.5f,220),"Raka","Kalian mau tontonan terakhir?! INI LAYAR TERAKHIR BUAT LO!",7);
            Shot(EndingId.TRUE,new Vector3(10,3,217),new Vector3(12,4,219),new Vector3(0,2,226),"Jepretan Terakhir","[Blitz memantul di antara cermin. Jaringan Penonton_Terakhir terbakar; Raka dan Damar terlempar ke arah subuh.]",8);
            Shot(EndingId.TRUE,centers[1]+new Vector3(-3,1.8f,-3),centers[1]+new Vector3(-2,1.7f,-1),centers[1]+new Vector3(.4f,1.4f,1.6f),"Damar","Ka... mulai besok, kita ganti channel aja ya. Bikin konten kuliner atau mukbang kek... jangan uji nyali lagi.",9,1);
            Shot(EndingId.TRUE,centers[1]+new Vector3(1,2,-2),centers[1]+new Vector3(1,6,-4),centers[1]+new Vector3(0,1.5f,3),"Raka","Setuju. Malam Keramat resmi tamat malam ini. [05:30 WIB. MataBatin.apk terhapus. Donasi tulus tersisa Rp125.000.000.]",10,1);
            Shot(EndingId.SECRET,new Vector3(18,1.9f,217),new Vector3(17,1.8f,218),new Vector3(16,1.6f,220),"Raka","Mereka bukan angka penonton. Mereka punya nama.",7);
            Shot(EndingId.SECRET,new Vector3(15,2,218),new Vector3(14,2,219),new Vector3(13,1.4f,220),"Damar","Rekam kesaksiannya, Ka. Biar kali ini mereka yang pulang.",7);
            Shot(EndingId.SECRET,centers[2]+new Vector3(-4,2,-3),centers[2]+new Vector3(-2,2,0),centers[2]+new Vector3(0,1.5f,14),"Arsip Saksi","Tari. Lastri dan anaknya. Sembilan anak buruh. Warga Karang Setra. Kesaksian bebas membatalkan klaim kontrak; korban lama meninggalkan jaringan ritual.",12,2);
            Shot(EndingId.SECRET,centers[2]+new Vector3(0,2,0),centers[2]+new Vector3(0,3,-2),centers[2]+new Vector3(0,1.2f,5),"Saksi Tanpa Layar","Raka dan Damar menyerahkan catatan korban kepada keluarga. Donasi sah Rp125.000.000. MataBatin.apk tak kembali. Kursi kosong; cermin biasa, tanpa wajah yang tertinggal.",12,2);
            d.endingShots=shots.ToArray();
            string creditsPath=Art+"Phase5Credits.txt";
            string credits="LAYAR TERAKHIR\nDesain game & cerita: pemilik proyek\n\nLISENSI ASET PHASE5\n\n";
            foreach(string path in new[]{"Audit/phase5-asset-credits.md","Audit/phase5-audio-credits.md"})if(File.Exists(path))credits+=File.ReadAllText(path)+"\n\n";
            File.WriteAllText(creditsPath,credits);AssetDatabase.ImportAsset(creditsPath);d.credits=AssetDatabase.LoadAssetAtPath<TextAsset>(creditsPath);
            d.world.epilogueRoot.SetActive(false);
        }
        private static void ParentNew(int before,Transform parent)
        {var children=new List<Transform>();for(int i=before;i<env.childCount;i++)children.Add(env.GetChild(i));foreach(var child in children)child.SetParent(parent,true);}
        private static void Lighting()
        {
            RenderSettings.skybox=null;RenderSettings.ambientMode=AmbientMode.Flat;RenderSettings.ambientLight=new Color(.085f,.065f,.085f);
            RenderSettings.fog=true;RenderSettings.fogMode=FogMode.Exponential;RenderSettings.fogColor=new Color(.075f,.018f,.029f);RenderSettings.fogDensity=.014f;
            d.phoneView.playerCamera.clearFlags=CameraClearFlags.SolidColor;d.phoneView.playerCamera.backgroundColor=RenderSettings.fogColor;
            var moon=Point("Starless_Red_Sky_Key",Vector3.zero).gameObject.AddComponent<Light>();moon.type=LightType.Directional;moon.transform.rotation=Quaternion.Euler(42,-32,0);moon.color=new Color(.48f,.55f,.68f);moon.intensity=.4f;moon.shadows=LightShadows.Soft;
            for(int side=-1;side<=1;side+=2)for(int i=0;i<6;i++)Lamp("MarketBulb_"+side+"_"+i,new Vector3(side*7,3.2f,8+i*8),new Color(1,.52f,.23f),1.5f,9);
            for(int i=0;i<5;i++)
            {
                var at=new Vector3(i%2==0?-3.5f:3.5f,3.2f,63+i*6);
                Lamp("FrozenNeon_"+i,at,new Color(.42f,.68f,.78f),1.7f,8);
                Model("lightbulb_led",at,.22f,0,"Bohlam fisik lorong beku "+i);
            }
            for(int i=0;i<4;i++)Lamp("BridgeAmber_"+i,new Vector3(6,2.7f,106+i*12),new Color(1,.37f,.18f),1.3f,8);
            for(int i=0;i<4;i++)Lamp("SewingPractical_"+i,new Vector3(i%2==0?-10:10,2.5f,160+i*7),new Color(.66f,.42f,.26f),.32f,6);
            var atmosphere=d.gameObject.AddComponent<LayarTerakhir.Phase5.Phase5Lighting>();atmosphere.playerCamera=d.phoneView.playerCamera;atmosphere.Apply();
            var volume=Point("PasarSetan_Film_Grade",Vector3.zero).gameObject.AddComponent<Volume>();volume.isGlobal=true;volume.priority=1;
            string path=Art+"PasarSetanVolume.asset";var profile=AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
            if(!profile){profile=ScriptableObject.CreateInstance<VolumeProfile>();AssetDatabase.CreateAsset(profile,path);}foreach(var c in profile.components.ToArray())Object.DestroyImmediate(c,true);profile.components.Clear();
            var tonemap=profile.Add<Tonemapping>();tonemap.mode.Override(TonemappingMode.ACES);
            var color=profile.Add<ColorAdjustments>();color.postExposure.Override(.65f);color.contrast.Override(18);color.saturation.Override(-14);
            var bloom=profile.Add<Bloom>();bloom.intensity.Override(.13f);bloom.threshold.Override(1.5f);
            var vignette=profile.Add<Vignette>();vignette.intensity.Override(.21f);vignette.smoothness.Override(.6f);volume.sharedProfile=profile;
            foreach(var c in profile.components)if(!AssetDatabase.Contains(c))AssetDatabase.AddObjectToAsset(c,profile);EditorUtility.SetDirty(profile);
        }
    }
}
#endif
