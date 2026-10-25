#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LayarTerakhir.Phase5;
using UnityEngine;
namespace LayarTerakhir.Editor
{
    public static partial class Phase5SceneBuilder
    {
        [Serializable] private sealed class SourceEntry{public string id,kind,function;}
        [Serializable] private sealed class SourceList{public SourceEntry[] sources;}
        private static void Dressing()
        {
            // Counters, storage, tools, medical equipment, and witnesses are
            // grouped by an authored function and stand on actual surfaces.
            for(int side=-1;side<=1;side+=2)
                for(int row=0;row<3;row++)
                {
                    Model("MarketStall",new Vector3(side*11,0,25+row*10),4.1f,side<0?90:-90,"Tenda pedagang pasar malam "+side+"_"+row,true);
                    Model("dining_table",new Vector3(side*7.8f,0,25+row*10),.8f,0,"Meja dagangan kios "+side+"_"+row,true);
                }
            // Merchant islands narrow the central sightline while preserving a
            // clear seven-metre pedestrian route and all archive/recovery access.
            for(int side=-1;side<=1;side+=2)for(int row=0;row<3;row++)
            {
                Vector3 at=new Vector3(side*5.5f,0,24+row*11);
                Model("dining_table",at,.8f,0,"Meja dagangan pulau pedestrian "+side+"_"+row,true);
                Model("plastic_crate_01",at+new Vector3(side*.15f,.82f,0),.28f,0,"Wadah dagangan pasar "+side+"_"+row);
                Model("lightbulb_led",at+Vector3.up*2.8f,.23f,0,"Bohlam dagangan fisik "+side+"_"+row);
                Lamp("MerchantWarm_"+side+"_"+row,at+Vector3.up*2.65f,new Color(1,.47f,.19f),1.3f,6);
            }
            // New stretcher geometry, never the Phase2 hospital kit.
            for(int i=0;i<3;i++)Model("MorgueStretcher",new Vector3(i%2==0?-4.8f:4.8f,0,70+i*6),.95f,90,"Brankar kamar mayat memori "+i,true);
            for(int i=0;i<3;i++)
            {
                Model(i==0?"GothicCabinet_01":i==1?"chinese_cabinet":"drawer_cabinet",new Vector3(i==1?-10:10,0,164+i*6),2.2f,i==1?90:-90,"Lemari penutup LOS ruang jahit "+i,true);
                Item("HIDE_CLOSET_"+i,new Vector3(i==1?-9:9,1,164+i*6),"Periksa sisi aman lemari");
            }
            for(int i=0;i<2;i++)
            {
                Model("SchoolDesk_01",new Vector3(i==0?10:-10,.25f,171),1.25f,90,"Meja kerja tinggi untuk merunduk "+i,true);
                Item("HIDE_TABLE_"+i,new Vector3(i==0?9:-9,.9f,171),"Masuk perlahan di bawah meja dengan jongkok");
            }
            var source=JsonUtility.FromJson<SourceList>(File.ReadAllText("Audit/phase5-asset-sources.json"));var indices=new Dictionary<string,int>();
            foreach(var entry in source.sources)
            {
                if(entry.kind!="model"||string.IsNullOrEmpty(entry.function)||usage.Any(u=>u.sourceId==entry.id)||!modelPaths.ContainsKey(entry.id))continue;
                string group=entry.function;if(!indices.ContainsKey(group))indices[group]=0;int i=indices[group]++;float height=Height(entry.id);
                Vector3 at;float yaw=0;string purpose;
                if(group=="GateMarket")
                {
                    int stall=i%6;int side=stall%2==0?-1:1;float z=25+(stall/2)*10;
                    bool small=height<.7f;at=small?new Vector3(side*(7.35f+(i%3)*.35f),.83f,z+(i/6%3-1)*.65f):new Vector3(side*15.2f,0,18+(i/2%7)*4.8f);
                    if(entry.id.ToLowerInvariant().Contains("chandelier")||entry.id.ToLowerInvariant().Contains("lightbulb"))at=new Vector3(side*8.4f,2.9f,z);
                    yaw=side<0?90:-90;purpose="Dagangan/perabot kios pasar • "+entry.id;
                }
                else if(group=="FrozenCorridor")
                {
                    int side=i%2==0?-1:1;at=new Vector3(side*5.1f,height<.65f?.84f:0,67+(i/2)*3);
                    if(entry.id=="GothicBed_01"){at=new Vector3(0,0,166);purpose="Fragmen kamar Atmaja dalam pulau ruang jahit";}
                    else purpose="Peralatan kamar mayat dan lemari kebersihan • "+entry.id;
                }
                else if(group=="BambooBridge")
                {
                    int side=i%2==0?-1:1;at=new Vector3(side*7.2f,height<.65f?.87f:0,106+(i/2)*4);yaw=90;purpose="Perawatan jembatan/pabrik yang terdistorsi • "+entry.id;
                }
                else if(group=="SewingMirror")
                {
                    int side=i%2==0?-1:1;at=new Vector3(side*10.5f,height<.65f?.82f:0,158+(i/2%7)*4);yaw=side<0?90:-90;purpose="Inventaris dan penyimpanan ruang jahit • "+entry.id;
                    if(entry.id.Contains("Chandelier"))at.y=2.8f;
                }
                else if(group=="AuctionStage")
                {int side=i%2==0?-1:1;at=new Vector3(side*17,0,205+(i/2)*4);purpose="Perangkat siaran dan proyeksi panggung lelang • "+entry.id;}
                else if(group=="BorderNature")
                {int side=i%2==0?-1:1;at=new Vector3(side*(20+i%3),0,18+(i/2)*9);height=entry.id.Contains("tree")&&!entry.id.Contains("stump")?7:entry.id.Contains("boulder")?2:height;purpose="Vegetasi dan sisa alam pada tepi pasar • "+entry.id;}
                else continue;
                if(height<.65f&&group!="GateMarket"&&group!="BorderNature")
                {
                    string table=group=="BambooBridge"?"steel_frame_shelves_03":"ClassicNightstand_01";
                    Model(table,new Vector3(at.x,0,at.z),.8f,yaw,"Penyimpanan/perabot penyangga "+entry.id,true);
                }
                var kit=new Dictionary<string,string>{{"modular_airduct_circular_01","AirductCircular"},{"modular_airduct_rectangular_01","AirductRectangular"},{"modular_metal_gutter","MetalGutter"},{"modular_pipes","PipeSteel"},{"modular_pipes_plastic_01","PipePlastic"}};
                if(kit.TryGetValue(entry.id,out string module))Module(module,at,new Vector3(.4f,1.5f,.4f),purpose,yaw,false);
                else Model(entry.id,at,height,yaw,purpose,height>=.65f&&group!="BorderNature");
            }
            // Bamboo is a separately downloaded source; no old forest clump is copied.
            for(int i=0;i<72;i++)
            {float z=99+(i%18)*3;int side=i%2==0?-1:1;Model("BambooMemory",new Vector3(side*(13+(i%4)*.6f),-1,z),5+(i%5)*.7f,i*137,"Bambu memori di luar pagar jembatan");}
            var ferris=Model("FerrisWheel",new Vector3(-28,0,35),32,0,"Bianglala berkarat berputar terbalik");
            var moving=ferris.GetComponentsInChildren<MeshRenderer>().Where(r=>!r.name.Contains("pCube151")).ToArray();
            Bounds wheelBounds=moving[0].bounds;foreach(var r in moving.Skip(1))wheelBounds.Encapsulate(r.bounds);
            var pivot=Point("FerrisReverseWheel",wheelBounds.center,ferris.transform);foreach(var r in moving)r.transform.SetParent(pivot,true);
            var mechanical=env.GetComponent<Phase5MechanicalPresentation>();mechanical.wheel=pivot;
            for(int i=0;i<24;i++)
            {
                Vector3 at;float yaw;
                if(i<12){int side=i%2==0?-1:1;at=new Vector3(side*5.7f,2+(i%3)*.7f,20+(i/2)*5);yaw=side<0?90:-90;}
                else if(i<15){at=new Vector3(i%2==0?-5.9f:5.9f,1.8f,68+(i-12)*9);yaw=i%2==0?90:-90;}
                else if(i<18){at=new Vector3(8.9f,1.6f,110+(i-15)*12);yaw=-90;}
                else if(i<21){at=new Vector3(-12,1.7f,162+(i-18)*9);yaw=90;}
                else{at=new Vector3(-16+(i-21)*16,2.2f,237);yaw=180;}
                var mirror=Model("AntiqueMirror",at,1.25f,yaw,"Cermin wajah penonton aktif "+i);
                var head=Model("ViewerHead",at+Vector3.up*.35f+Quaternion.Euler(0,yaw,0)*Vector3.forward*.09f,.55f,yaw,"Wajah manekin saksi tanpa identitas nyata "+i);
                head.transform.SetParent(mirror.transform,true);faces.Add(mirror);
            }
            mechanical.mirrors=faces.Select(f=>f.transform).ToArray();
            for(int i=0;i<28;i++)
            {
                float angle=i*Mathf.PI*2/28;var at=new Vector3(Mathf.Sin(angle)*24,4+(i%4),220+Mathf.Cos(angle)*24);
                Module("MirrorScreen",at,new Vector3(2.6f,1.5f,.04f),"Layar proyeksi melayang tanpa tripod "+i,angle*Mathf.Rad2Deg+180,false);
                Label("ScreenChat"+i,i%3==0?"JANGAN MATIKAN SIARAN":i%3==1?"KAMI INGIN MELIHAT LELANGNYA":"SATU JUTA MATA",at+Vector3.up*1.3f,angle*Mathf.Rad2Deg+180,.009f);
            }
        }
        private static float Height(string id)
        {
            string key=id.ToLowerInvariant();
            var exact=new Dictionary<string,float>{{"barrel_01",.95f},{"barrel_02",.85f},{"barrel_03",.9f},{"coffeecart_01",1.8f},{"cashregister_01",.38f},{"cheesebox_01",.2f},{"megaphone_01",.35f},{"dining_table",.8f},{"outdoor_table_chair_set_01",1.1f},{"wooden_picnic_table",.85f},{"street_lamp_01",4},{"street_lamp_02",4},{"television_01",.65f},{"television_02",.65f},{"gothicbed_01",1.3f},{"barbershopchair_01",1.35f},{"steel_frame_shelves_03",2},{"old_drill_press",1.7f}};
            if(exact.TryGetValue(key,out float value))return value;
            if(key.Contains("cigarette")||key.Contains("pocket_watch")||key.Contains("lightbulb"))return .12f;
            if(key.Contains("lemon")||key.Contains("kiwi")||key.Contains("rubber_duck")||key.Contains("food_can"))return .14f;
            if(key.Contains("cabinet")||key.Contains("shelves")||key.Contains("bookshelf")||key.Contains("grandfather"))return 1.9f;
            if(key.Contains("chair")||key.Contains("stool")||key.Contains("armchair")||key.Contains("sofa"))return 1;
            if(key.Contains("table")||key.Contains("commode")||key.Contains("console"))return .8f;
            if(key.Contains("lamp")||key.Contains("chandelier"))return .65f;
            if(key.Contains("tree")&&!key.Contains("stump"))return 6;
            if(key.Contains("trunk")||key.Contains("boulder"))return 2.1f;
            if(key.Contains("stump")||key.Contains("crates")||key.Contains("tyre")||key.Contains("tank")||key.Contains("crate"))return .7f;
            if(key.Contains("cleaner")||key.Contains("bleach")||key.Contains("bottle")||key.Contains("can_"))return .28f;
            return .4f;
        }
    }
}
#endif
