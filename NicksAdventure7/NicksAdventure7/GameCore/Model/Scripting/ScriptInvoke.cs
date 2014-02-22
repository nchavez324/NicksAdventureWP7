using NicksAdventure7.GameCore.Model.Entities;
using NicksAdventure7.GameCore.Model.Entities.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NicksAdventure7.GameCore.Model.Scripting;

namespace NicksAdventure7.GameCore.Model.Scripting{
    class ScriptInvoke{
        //methods and shit for loading each map and stuff
        public static ScriptedEvent[, ,] GetScriptedEvents(string mapName, int[] mapDims){
            ScriptedEvent[,,] ans = new ScriptedEvent[mapDims[0],mapDims[1],mapDims[2]];
            if (mapName.CompareTo("DemoMap") == 0){

                //Yale_Jasmine
                ans[2, 3, 0] = new ScriptedEvent(new EventPage[1] {
                    new EventPage("Images/Characters/Jasmine", new int[3]{ 2, 3, 0}, Entities.Terrain.Tile.Direction.SOUTH,
                        new Movement(Movement.Type.FIXED, Movement.Trigger.ACTION, true, 1),
                        (player) => {
                            Global.action.dialog.CallDialog(
                                "Hi! My name is Jasmine. I go to Yale, but I'm here visiting Harvard. " +
                                "I have one question though -- do people pee on the John Harvard Statue? " +
                                "Because, I'll be honest, I saw people peeing on that staue last night!! <<Yes;No>>",
                                (response)=>{
                                    if (response.CompareTo("Yes") == 0){
                                        return Global.action.dialog.CallDialog("Yuck! I knew it... ", null);
                                    }else{
                                        return Global.action.dialog.CallDialog("Really? I don't believe you! ", null);
                                    }
                                });
                        }, "Yale_Jasmine") }, new int[3] { 2, 3, 0 }, "Yale_Jasmine");
                //Princeton_Jasmine
                ans[4, 3, 0] = new ScriptedEvent(new EventPage[1] {
                    new EventPage("Images/Characters/Jasmine", new int[3]{ 4, 3, 0}, Entities.Terrain.Tile.Direction.EAST,
                        new Movement(Movement.Type.LOOK_AROUND, Movement.Trigger.ACTION, true, 1),
                        (player) => {
                            bool[] plyrAns = new bool[4];
                            Global.action.dialog.CallDialog(
                                "Hi! My name is also Jasmine. I go to Princeton, but I'm here visiting Harvard. " +
                                "I have a series of questions to ask you: Are you Nick? <<YUP;NOPE>>",
                                (r0)=>{
                                    plyrAns[0] = (r0.CompareTo("YUP") == 0);
                                    return Global.action.dialog.CallDialog(
                                        "And, for this question, please do be honest, we're trying to minimize bias: " + 
                                        "Do you like Harvard? <<YUP;NOPE>>",
                                        (r1)=>{
                                            plyrAns[1] = (r1.CompareTo("YUP") == 0);
                                            return Global.action.dialog.CallDialog(
                                                "Do you like Yale? <<YUP;NOPE>>",
                                                (r2)=>{
                                                    plyrAns[2] = (r2.CompareTo("YUP") == 0);
                                                    return Global.action.dialog.CallDialog(
                                                        "Lastly, are you from the West Coast? <<YUP;NOPE>>",
                                                        (r3)=>{
                                                            plyrAns[3] = (r3.CompareTo("YUP") == 0);
                                                            return Global.action.dialog.CallDialog(
                                                                "Therefore: you are " + (plyrAns[0]?"":"not ") + "Nick, you do " +
                                                                (plyrAns[1]?"":"not ") + "like Harvard, you do " + (plyrAns[2]?"":"not ") + "like Yale, and lastly, " +
                                                                "are " + (plyrAns[3]?"":"not ") + "from the West Coast! ",
                                                                null);
                                                        }
                                                        );
                                                });
                                        });
                                });
                        }, "Princeton_Jasmine") }, new int[3] { 4, 3, 0 }, "Princeton_Jasmine");
                //Crunch Time
                ans[5, 5, 0] = new ScriptedEvent(new EventPage[1] {
                    new EventPage("Images/Characters/Jasmine", new int[3]{ 5, 5, 0}, Entities.Terrain.Tile.Direction.WEST,
                        new Movement(Movement.Type.WALK_AROUND, Movement.Trigger.ACTION, true, 1),
                        (player) => {
                            Global.action.dialog.CallDialog(
                                "At what point do you start seeing bread? " +
                                "Been out hustling for years, shoe box right under my bed " + 
                                "I move the work out my momma's house, got me a little old crib " +
                                "I always fantasize if I had went to college instead " +
                                "Would I be happily married instead of broke and unwed? " +
                                "My nigga made a major move I said I hope for the best " +
                                "I told my sister as I kissed her cheek I'm better off dead " +
                                "Fucking with this white, it's all been downhill like a sled " +
                                "Now listen, I understand they say you make your own bed " +
                                "But tell me who supplied these sheets with this cheap ass thread " +
                                "In denial about the feds, he can't see past bread " +
                                "Now do exactly what the man in the ski mask says, okay? " +
                                "These are the times, survival my only crime " +
                                "I gotta be on my grind, a lot of my homies gone " +
                                "Inside of me Lord I know, it's a lie that we gon' be fine " +
                                "But momma I'm tired of crying, just lie to me one more time, " +
                                "Cause it's Crunch Time ... ",
                                null);
                        }, "Crunch Time") }, new int[3] { 5, 5, 0 }, "Crunch Time");

            }
            return ans;
        }
        public static int GetNumEvents(string mapName){
            int ans = 0;
            if (mapName.CompareTo("DemoMap") == 0) ans = 3;

            return ans;
        }
    }
}
