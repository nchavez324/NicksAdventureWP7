using NicksAdventure7.GameCore.Util;
using Microsoft.Xna.Framework;
using NicksAdventure7.GameCore.Model.Entities.Events;
using NicksAdventure7.GameCore.Model.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NicksAdventure7.GameCore.Model.Entities.Terrain{
    class TileMapParser{
        Dictionary<string, string> HardCodedMapData;
        public TileMapParser(){
            HardCodedMapData = new Dictionary<string, string>();
            //Maps
            //Map(<NameOfMap>,[<mapW>,<mapL>,<mapH>], <numNonNullTiles>, <numEvents>, <numBuildings>)
            HardCodedMapData.Add("DemoMap",
                "Map(DemoMap,[9,8,3],64,1,1);" +

                "T[0,0,0](INNER_CORNER,{0,0,0},WEST,SOUTH);" +
                "T[0,0,1](INNER_CORNER,{0,0,0},SOUTH,WEST);"+
                "T[1,0,0](SLOPE,{1,0,0},NORTH);" +
                "T[2,0,0](SLOPE,{2,0,0},NORTH);" +
                "T[3,0,0](SLOPE,{3,0,0},NORTH);" +
                "T[4,0,0](SLOPE,{4,0,0},NORTH);" +
                "T[5,0,0](SLOPE,{5,0,0},NORTH);" +

                "T[3,4,0](OUTER_CORNER,{3,4,0},SOUTH,EAST);" +
                "T[3,4,1](OUTER_CORNER,{3,4,0},EAST,SOUTH);" +
                "T[2,4,0](SLOPE,{2,4,0},SOUTH);" +
                "T[1,4,0](SLOPE,{1,4,0},SOUTH);" +

                "T[0,4,0](INNER_CORNER,{0,4,0},NORTH,WEST);" +
                "T[0,4,1](INNER_CORNER,{0,4,0},WEST,NORTH);" +
                "T[0,3,0](SLOPE,{0,3,0},EAST);" +
                "T[0,2,0](SLOPE,{0,2,0},EAST);" +
                "T[0,1,0](SLOPE,{0,1,0},EAST);" +

                "T[3,5,0](SLOPE,{3,5,0},EAST);" +
                "T[3,6,0](SLOPE,{3,6,0},EAST);" +

                "T[1,1,0](FLAT,{1,1,0});" +
                "T[2,1,0](FLAT,{2,1,0});" +
                "T[3,1,0](FLAT,{3,1,0});" +
                "T[1,2,0](FLAT,{1,2,0});" +
                "T[2,2,0](FLAT,{2,2,0});" +
                "T[3,2,0](FLAT,{3,2,0});" +
                "T[1,3,0](FLAT,{1,3,0});" +
                "T[2,3,0](FLAT,{2,3,0});" +
                "T[3,3,0](FLAT,{3,3,0});" +

                "T[4,1,0](FLAT,{4,1,0});" +
                "T[4,2,0](FLAT,{4,2,0});" +
                "T[4,3,0](FLAT,{4,3,0});" +
                "T[5,1,0](FLAT,{5,1,0});" +
                "T[5,2,0](FLAT,{5,2,0});" +
                "T[5,3,0](FLAT,{5,3,0});" +

                "T[4,4,0](FLAT,{4,4,0});" +
                "T[4,5,0](FLAT,{4,5,0});" +
                "T[4,6,0](FLAT,{4,6,0});" +
                "T[5,4,0](FLAT,{5,4,0});" +
                "T[5,5,0](FLAT,{5,5,0});" +
                "T[5,6,0](FLAT,{5,6,0});" +

                "T[6,4,0](FLAT,{6,4,0});" +
                "T[7,4,0](FLAT,{7,4,0});" +
                "T[8,4,0](FLAT,{8,4,0});" +
                "T[6,5,0](FLAT,{6,5,0});" +
                "T[7,5,0](FLAT,{7,5,0});" +
                "T[8,5,0](FLAT,{8,5,0});" +
                "T[6,6,0](FLAT,{6,6,0});" +
                "T[7,6,0](FLAT,{7,6,0});" +
                "T[8,6,0](FLAT,{8,6,0});" +

                "T[5,1,1](VERTICAL,{5,1,0},EAST);" +
                "T[5,2,1](VERTICAL,{5,2,0},EAST);" +
                "T[5,3,1](VERTICAL,{5,3,0},EAST);" +
                "T[6,4,1](VERTICAL,{6,4,0},SOUTH);" +
                "T[7,4,1](VERTICAL,{7,4,0},SOUTH);" +
                "T[8,4,1](VERTICAL,{8,4,0},SOUTH);" +
                "T[8,4,2](VERTICAL,{8,4,0},EAST);" +
                "T[8,5,1](VERTICAL,{8,5,0},EAST);" +
                "T[8,6,1](VERTICAL,{8,6,0},EAST);" +
                "T[4,6,1](VERTICAL,{5,6,0},NORTH);" +
                "T[5,6,1](VERTICAL,{4,6,0},NORTH);" +
                "T[6,6,1](VERTICAL,{6,6,0},NORTH);" +
                "T[7,6,1](VERTICAL,{7,6,0},NORTH);" +
                "T[8,6,2](VERTICAL,{8,6,0},NORTH);" +

                "T[3,6,1](SV_CORNER,{3,6,0},NORTH,WEST);"+
                "T[5,0,1](SV_CORNER,{5,0,0},EAST,SOUTH);" +
                
                "B(BlockHouse,{5,7,0});"+
                
                "SD('Images/Events/slidingDoor',{6,6,0},{1,6,1},SOUTH,SOUTH,[],'StairMap');"
                );

            HardCodedMapData.Add("StairMap",
                "Map(StairMap,[5,8,2],39,1,1);" +

                "T[0,0,0](FLAT,{0,0,0});" +
                "T[1,0,0](FLAT,{1,0,0});" +
                "T[2,0,0](FLAT,{2,0,0});" +
                "T[3,0,0](FLAT,{3,0,0});" +
                "T[0,1,0](FLAT,{0,1,0});" +
                "T[1,1,0](FLAT,{1,1,0});" +
                "T[2,1,0](FLAT,{2,1,0});" +
                "T[3,1,0](FLAT,{3,1,0});" +
                "T[0,2,0](FLAT,{0,2,0});" +
                "T[1,2,0](FLAT,{1,2,0});" +
                "T[2,2,0](FLAT,{2,2,0});" +
                "T[3,2,0](FLAT,{3,2,0});" +

                "T[0,3,0](SLOPE,{0,3,0},SOUTH);" +
                "T[1,3,0](SLOPE,{1,3,0},SOUTH);" +
                "T[2,3,0](SLOPE,{2,3,0},SOUTH);" +
                "T[3,3,0](SLOPE,{3,3,0},SOUTH);" +

                "T[0,4,0](FLAT,{0,4,1});" +
                "T[1,4,0](FLAT,{1,4,1});" +
                "T[2,4,0](FLAT,{2,4,1});" +
                "T[3,4,0](FLAT,{3,4,1});" +
                "T[0,5,0](FLAT,{0,5,1});" +
                "T[1,5,0](FLAT,{1,5,1});" +
                "T[2,5,0](FLAT,{2,5,1});" +
                "T[3,5,0](FLAT,{3,5,1});" +
                "T[0,6,0](FLAT,{0,6,1});" +
                "T[1,6,0](FLAT,{1,6,1});" +
                "T[2,6,0](FLAT,{2,6,1});" +
                "T[3,6,0](FLAT,{3,6,1});" +

                "T[4,3,0](OUTER_CORNER,{4,3,0},SOUTH,EAST);" +
                "T[4,3,1](OUTER_CORNER,{4,3,0},EAST,SOUTH);" +
                "T[4,4,0](SLOPE,{4,4,0},EAST);" +
                "T[4,5,0](SLOPE,{4,5,0},EAST);" +
                "T[4,6,0](SLOPE,{4,6,0},EAST);" +

                "T[4,7,0](OUTER_CORNER,{4,7,0},EAST,NORTH);" +
                "T[4,7,1](OUTER_CORNER,{4,7,0},NORTH,EAST);" +
                "T[3,7,0](SLOPE,{3,7,0},NORTH);" +
                "T[2,7,0](SLOPE,{2,7,0},NORTH);" +
                "T[1,7,0](SLOPE,{1,7,0},NORTH);" +
                "T[0,7,0](SLOPE,{0,7,0},NORTH);" +

                "B(BlockHouse,{0,7,1});" +

                "SD('Images/Events/slidingDoor',{1,6,1},{6,6,0},SOUTH,SOUTH,[],'DemoMap');"          
                );
        }
        public TileMap LoadMap(string mapName){
            //extract file
            return ParseFile(ToHashSet(HardCodedMapData[mapName], ';'));
        }
        private TileMap ParseFile(HashSet<string> input){
            Tile[, ,] tiles;
            IEvent[,,] events;
            Building[] buildings;
            ScriptedEvent[, ,] se;
            string mapName, line;
            int mapL, mapW, mapH, numNonNull, numEvents, numBuildings, buildingNum = 0;


            line = input.ElementAt(0);
            line = line.Substring(line.IndexOf('(') + 1).Trim();
            mapName = line.Substring(0, line.IndexOf(','));
            line = line.Substring(line.IndexOf('[') + 1).Trim();
            mapL = System.Convert.ToInt32(line.Substring(0, line.IndexOf(',')));
            line = line.Substring(line.IndexOf(',') + 1).Trim();
            mapW = System.Convert.ToInt32(line.Substring(0, line.IndexOf(',')));
            line = line.Substring(line.IndexOf(',') + 1).Trim();
            mapH = System.Convert.ToInt32(line.Substring(0, line.IndexOf(']')));
            line = line.Substring(line.IndexOf(',') + 1).Trim();
            numNonNull = System.Convert.ToInt32(line.Substring(0, line.IndexOf(',')));
            line = line.Substring(line.IndexOf(',') + 1).Trim();
            numEvents = System.Convert.ToInt32(line.Substring(0, line.IndexOf(',')));
            line = line.Substring(line.IndexOf(',') + 1).Trim();
            numBuildings = System.Convert.ToInt32(line.Substring(0, line.IndexOf(')')));

            tiles = new Tile[mapL, mapW, mapH];
            events = new IEvent[mapL, mapW, mapH];
            buildings = new Building[numBuildings];
            se = ScriptInvoke.GetScriptedEvents(mapName, new int[3] { events.GetLength(0), events.GetLength(1), events.GetLength(2) });
            numEvents += ScriptInvoke.GetNumEvents(mapName);

            //Basic Data down!

            for (int i = 1; i < input.Count; i++){
                line = input.ElementAt(i).Trim();
                
                if (line[0] == 'T'){
                    int[] arrayPos = new int[3];
                    int[] mapPos = new int[3];
                    Tile.TileStyle style = Tile.TileStyle.FLAT;
                    Tile.Direction dir, secondDir;
                    line = line.Substring(line.IndexOf('[') + 1).Trim();

                    arrayPos[0] = System.Convert.ToInt32(line.Substring(0, line.IndexOf(',')).Trim());
                    line = line.Substring(line.IndexOf(',') + 1).Trim();
                    arrayPos[1] = System.Convert.ToInt32(line.Substring(0, line.IndexOf(',')).Trim());
                    line = line.Substring(line.IndexOf(',') + 1).Trim();
                    arrayPos[2] = System.Convert.ToInt32(line.Substring(0, line.IndexOf(']')).Trim());
                    line = line.Substring(line.IndexOf('(') + 1).Trim();

                    style = (Tile.TileStyle)Enum.Parse(typeof(Tile.TileStyle),  
                        line.Substring(0, line.IndexOf(',')).Trim(), true);
                    line = line.Substring(line.IndexOf('{') + 1).Trim();
                    mapPos[0] = System.Convert.ToInt32(line.Substring(0, line.IndexOf(',')).Trim());
                    line = line.Substring(line.IndexOf(',') + 1).Trim();
                    mapPos[1] = System.Convert.ToInt32(line.Substring(0, line.IndexOf(',')).Trim());
                    line = line.Substring(line.IndexOf(',') + 1).Trim();
                    mapPos[2] = System.Convert.ToInt32(line.Substring(0, line.IndexOf('}')).Trim());

                    if (line.IndexOf(',') == -1){
                        tiles[arrayPos[0], arrayPos[1], arrayPos[2]] = new Tile(
                            style,
                            mapPos,
                            null,
                            null);
                    }else{
                        line = line.Substring(line.IndexOf(',') + 1).Trim();
                        if (line.IndexOf(',') == -1){
                            dir = (Tile.Direction)Enum.Parse(typeof(Tile.Direction), line.Substring(0, line.IndexOf(')')).Trim(), true);
                            tiles[arrayPos[0], arrayPos[1], arrayPos[2]] = new Tile(
                            style,
                            mapPos,
                            dir,
                            null);
                        }else{
                            dir = (Tile.Direction)Enum.Parse(typeof(Tile.Direction), line.Substring(0, line.IndexOf(',')).Trim(), true);
                            line = line.Substring(line.IndexOf(',') + 1).Trim();
                            if (line.IndexOf(',') == -1){
                                secondDir = (Tile.Direction)Enum.Parse(typeof(Tile.Direction), line.Substring(0, line.IndexOf(')')).Trim(), true);
                                tiles[arrayPos[0], arrayPos[1], arrayPos[2]] = new Tile(
                                style,
                                mapPos,
                                dir,
                                secondDir);
                            }else{
                                secondDir = (Tile.Direction)Enum.Parse(typeof(Tile.Direction), line.Substring(0, line.IndexOf(',')).Trim(), true);
                                line = line.Substring(line.IndexOf('{') + 1).Trim();
                                bool[] passage = new bool[4];
                                passage[0] = System.Convert.ToBoolean(line.Substring(0, line.IndexOf(',')).Trim());
                                line = line.Substring(line.IndexOf(',') + 1).Trim();
                                passage[1] = System.Convert.ToBoolean(line.Substring(0, line.IndexOf(',')).Trim());
                                line = line.Substring(line.IndexOf(',') + 1).Trim();
                                passage[2] = System.Convert.ToBoolean(line.Substring(0, line.IndexOf(',')).Trim());
                                line = line.Substring(line.IndexOf(',') + 1).Trim();
                                passage[3] = System.Convert.ToBoolean(line.Substring(0, line.IndexOf('}')).Trim());
                                if(line.IndexOf(',') == -1){
                                    tiles[arrayPos[0], arrayPos[1], arrayPos[2]] = new Tile(
                                        style,
                                        mapPos,
                                        dir,
                                        secondDir
                                        );
                                        tiles[arrayPos[0], arrayPos[1], arrayPos[2]].passage = passage;
                                    }else{
                                        line = line.Substring(line.IndexOf('\'')).Trim();
                                        string tPath = line.Substring(0, line.IndexOf('\'')).Trim();
                                        tiles[arrayPos[0], arrayPos[1], arrayPos[2]] = new Tile(
                                            style,
                                            tPath,
                                            mapPos,
                                            passage,
                                            dir,
                                            secondDir
                                            );
                                    }
                                }
                            }
                        }

                }else if (line[0] == 'C'){
                    //make me a character!
                    string texturePath = "";
                    int[] mapPos = new int[3];
                    Tile.Direction dir;

                    line = line.Substring(line.IndexOf('\'') + 1).Trim();
                    texturePath = line.Substring(0, line.IndexOf('\'')).Trim();
                    line = line.Substring(line.IndexOf('{') + 1).Trim();

                    mapPos[0] = System.Convert.ToInt32(line.Substring(0, line.IndexOf(',')).Trim());
                    line = line.Substring(line.IndexOf(',') + 1).Trim();
                    mapPos[1] = System.Convert.ToInt32(line.Substring(0, line.IndexOf(',')).Trim());
                    line = line.Substring(line.IndexOf(',') + 1).Trim();
                    mapPos[2] = System.Convert.ToInt32(line.Substring(0, line.IndexOf('}')).Trim());
                    line = line.Substring(line.IndexOf(',') + 1).Trim();

                    dir = (Tile.Direction)Enum.Parse(typeof(Tile.Direction), line.Substring(0, line.IndexOf(')')).Trim(), true);
                    events[mapPos[0], mapPos[1], mapPos[2]] = new OWPlayer(texturePath, mapPos, dir);
                }else if (line[0] == 'S' && line[1] == 'D'){
                    //make me a sliding door!
                    //SD('Images/Events/slidingDoor',{3,2,0},SOUTH,[],'stairMap');
                    string texturePath = "";
                    int[] mapPos = new int[3];
                    int[] destPos = new int[3];
                    Tile.Direction dir, playerDir;
                    bool givenSect;
                    Rectangle section = new Rectangle(0, 0, 0, 0);
                    string mapDestKey = "";

                    line = line.Substring(line.IndexOf('\'') + 1).Trim();
                    texturePath = line.Substring(0, line.IndexOf('\'')).Trim();
                    line = line.Substring(line.IndexOf('{') + 1).Trim();

                    mapPos[0] = System.Convert.ToInt32(line.Substring(0, line.IndexOf(',')).Trim());
                    line = line.Substring(line.IndexOf(',') + 1).Trim();
                    mapPos[1] = System.Convert.ToInt32(line.Substring(0, line.IndexOf(',')).Trim());
                    line = line.Substring(line.IndexOf(',') + 1).Trim();
                    mapPos[2] = System.Convert.ToInt32(line.Substring(0, line.IndexOf('}')).Trim());
                    line = line.Substring(line.IndexOf('{') + 1).Trim();

                    destPos[0] = System.Convert.ToInt32(line.Substring(0, line.IndexOf(',')).Trim());
                    line = line.Substring(line.IndexOf(',') + 1).Trim();
                    destPos[1] = System.Convert.ToInt32(line.Substring(0, line.IndexOf(',')).Trim());
                    line = line.Substring(line.IndexOf(',') + 1).Trim();
                    destPos[2] = System.Convert.ToInt32(line.Substring(0, line.IndexOf('}')).Trim());
                    line = line.Substring(line.IndexOf(',') + 1).Trim();

                    dir = (Tile.Direction)Enum.Parse(typeof(Tile.Direction), line.Substring(0, line.IndexOf(',')).Trim(), true);
                    line = line.Substring(line.IndexOf(',') + 1).Trim();
                    playerDir = (Tile.Direction)Enum.Parse(typeof(Tile.Direction), line.Substring(0, line.IndexOf(',')).Trim(), true);
                    line = line.Substring(line.IndexOf('[') + 1).Trim();

                    givenSect = (line[0] != ']');

                    if (givenSect){
                        float x = System.Convert.ToSingle(line.Substring(0,line.IndexOf(',')).Trim());
                        line = line.Substring(line.IndexOf(',') + 1).Trim();
                        float y = System.Convert.ToSingle(line.Substring(0, line.IndexOf(',')).Trim());
                        line = line.Substring(line.IndexOf(',') + 1).Trim();
                        float w = System.Convert.ToSingle(line.Substring(0, line.IndexOf(',')).Trim());
                        line = line.Substring(line.IndexOf(',') + 1).Trim();
                        float h = System.Convert.ToSingle(line.Substring(0, line.IndexOf(']')).Trim());
                        section = new Rectangle((int)x, (int)y, (int)w, (int)h);
                    }
                    line = line.Substring(line.IndexOf('\'') + 1).Trim();
                    mapDestKey = line.Substring(0, line.IndexOf('\'')).Trim();

                    if (givenSect)
                        events[mapPos[0], mapPos[1], mapPos[2]] = new SlidingDoor(texturePath, mapPos, destPos, dir, playerDir, section, mapDestKey);
                    else
                        events[mapPos[0], mapPos[1], mapPos[2]] = new SlidingDoor(texturePath, mapPos, destPos, dir, playerDir, mapDestKey);
                }else if (line[0] == 'B'){
                    //make me a building
                    //"B(Block264Tex,{5,6,0});"+
                    Name name;
                    int[] mapPos = new int[3];
                    line = line.Substring(line.IndexOf('(') + 1).Trim();
                    string str = line.Substring(0, line.IndexOf(',')).Trim();
                    name = (Name)Enum.Parse(typeof(Name), str, true);
                    line = line.Substring(line.IndexOf('{') + 1).Trim();
                    mapPos[0] = System.Convert.ToInt32(line.Substring(0, line.IndexOf(',')).Trim());
                    line = line.Substring(line.IndexOf(',') + 1).Trim();
                    mapPos[1] = System.Convert.ToInt32(line.Substring(0, line.IndexOf(',')).Trim());
                    line = line.Substring(line.IndexOf(',') + 1).Trim();
                    mapPos[2] = System.Convert.ToInt32(line.Substring(0, line.IndexOf('}')).Trim());

                    buildings[buildingNum] = new Building(name, mapPos);
                    buildingNum++;
                }
            }
            
            for (int i = 0; i < se.GetLength(0); i++){
                for (int j = 0; j < se.GetLength(1); j++){
                    for (int k = 0; k < se.GetLength(2); k++){
                        if (se[i, j, k] != null)
                            events[i, j, k] = se[i, j, k];
                    }
                }
            }
            return new TileMap(mapName, tiles, events, buildings, numNonNull, numEvents);
        }
        private HashSet<string> ToHashSet(string input, char seperator){
            HashSet<string> output = new HashSet<string>();
            string[] lines = input.Split(new char[1] { seperator });
            for (int i = 0; i < lines.Length; i++)
                if(lines[i].CompareTo("") != 0)
                    output.Add(lines[i]);
            return output;
        }
    }
}
