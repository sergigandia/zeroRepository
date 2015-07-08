using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace MasterOfInsecRework
{
    class Modes
    {
        private Obj_AI_Hero target;
        private Program p;
        private Skills skills;
        private int LastPlaced = new int();
        private Vector3 wardPosition = new Vector3();
        private int SecondWTime = new int();
        private float lastWardjump = 0;

        private bool beforeall = true;
        private int secondQTime = new int();
        private Obj_AI_Hero insecAlly;
        private Obj_AI_Hero insecEnemy;

        private bool oldPositionbool = false;


        public void load(Program p)
        {
            skills = new Skills();
            this.p = p;
            target = TargetSelector.GetTarget(1300, TargetSelector.DamageType.Physical);
        }
        public Obj_AI_Hero getTarget()
        {
            target = TargetSelector.GetTarget(1300, TargetSelector.DamageType.Physical);
            return target;
        }
        public void setInsecEnemy(Obj_AI_Hero x)
        {
            this.insecEnemy = x;
        }
        public bool getOldPositionbool()
        {
            return oldPositionbool;
        }
        public void setOldPositionbool(bool x)
        {
            this.oldPositionbool = x;
        }
        public void setInsecAlly(Obj_AI_Hero x)
        {
            this.insecEnemy = x;
        }
        
        public Skills getSkills()
        {

            return skills;
        }


        //combos   
        public void laneClear()
        {
            var minion = MinionManager.GetMinions(skills.getQ().Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth).FirstOrDefault();
            var useQ = p.getMenu().Item("QJ").GetValue<bool>();      
        }
        public void jungleClear()
        {
            var minion = MinionManager.GetMinions(skills.getQ().Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth).FirstOrDefault();
            var useQ = p.getMenu().Item("QJ").GetValue<bool>();
        }
        public void harrash(Obj_AI_Hero target)
        {
            var useQ = p.getMenu().Item("QH").GetValue<bool>();
            var useW = p.getMenu().Item("WH").GetValue<bool>();
            var useE = p.getMenu().Item("EH").GetValue<bool>();
            var hit = p.getMenu().Item("seth").GetValue<Slider>().Value;
        }
        public void items()
        {
            if (Items.CanUseItem(3077) && p.getPlayer().Distance(target.Position) < 350)
                Items.UseItem(3077);
            if (Items.CanUseItem(3074) && p.getPlayer().Distance(target.Position) < 350)
                Items.UseItem(3074);
            if (Items.CanUseItem(3142) && p.getPlayer().Distance(target.Position) < 350)
                Items.UseItem(3142);
        }
        public void killeableR()
        {
            if (skills.getE().IsReady() && skills.getE().IsKillable(target)) // si la e mata
            {
                skills.getE().Cast(target);
            }
            else if (skills.getR().IsReady() && skills.getR().IsKillable(target)) // si solo la r mata
            {
                skills.getR().Cast(target);
            }
            else if (skills.getIgnite().IsReady() && skills.getR().IsReady() && p.getMenu().Item("IgniteR").GetValue<bool>()) // si ignite R mata
            {
                double DamageRIgnite = ObjectManager.Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite) +
                    p.getPlayer().GetSpellDamage(target, SpellSlot.R);
                if (target.Health - DamageRIgnite <= 0)
                {
                    skills.rCast(target);
                    ObjectManager.Player.Spellbook.CastSpell(skills.getIgnite(), target);
                }
            }
        }
        public void killeableWithoutR()
        {
            if (skills.getE().IsReady() && skills.getE().IsKillable(target)) // si la e mata
            {
                skills.getE().Cast(target);
            }
            else if (skills.getIgnite().IsReady() && skills.getR().IsReady() && p.getMenu().Item("IgniteR").GetValue<bool>()) // si ignite R mata
            {
                double DamageRIgnite = ObjectManager.Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite) +
                    p.getPlayer().GetSpellDamage(target, SpellSlot.R);
                if (target.Health - DamageRIgnite <= 0)
                {
                    skills.rCast(target);
                    ObjectManager.Player.Spellbook.CastSpell(skills.getIgnite(), target);
                }
            }
        }
       

        public void combo(Obj_AI_Hero target)
        {
           
            var useQ = p.getMenu().Item("comboQ").GetValue<bool>();
            var useW = p.getMenu().Item("comboW").GetValue<bool>();
            var useE = p.getMenu().Item("comboE").GetValue<bool>();
            var useR = p.getMenu().Item("comboR").GetValue<bool>();            
           
            var hit = p.getMenu().Item("seth").GetValue<Slider>().Value;
            var vidaMin = p.getMenu().Item("Set W life %").GetValue<Slider>().Value;
            

            //Faltan casos con r on 

            if (useQ && !useW && !useE && !useR)
                skills.qCast(target, hit);
            else if (!useQ && useW && !useE && !useR)
                skills.wCast(target);
            else if (!useQ && !useW && useE && !useR)
                skills.eCast(target);
            else if (!useQ && useW && useE && !useR)
            {
                skills.wCast(target);
                skills.eCast(target);
            }
            else if (useQ && !useW && useE && !useR)
            {
                skills.qCast(target, hit);
                skills.eCast(target);
            }
            else if (useQ && useW && !useE && !useR)
            {
                skills.qCast(target, hit);
                skills.wCast(target);
            }
            else if (useQ && !useW && useE && !useR)
            {
                if (p.getMenu().Item("comboWLH").GetValue<bool>() && ObjectManager.Player.HealthPercent <= vidaMin)
                    skills.wCast(p.getPlayer());

                if (skills.qCast(target, hit))
                    Utility.DelayAction.Add(700, () => skills.qCast2(target));

                //Pegar un blanco ¡¡¡¡¡¡¡¡¡¡¡¡IMPORTANTE!!!!!!!!!!!!!!!
                //work 
                #region work
                if (skills.eCast(target))
                {
                    //Pegar un blanco ¡¡¡¡¡¡¡¡¡¡¡¡IMPORTANTE!!!!!!!!!!!!!!!
                    skills.eCast(target);
                    items();  
                }                
                                  
                //Aqui solamente entrar cuando mato al enemigo con las skills que tengo activas en ese momento.
                killeableWithoutR();
               
                if (skills.getIgnite().IsReady()) // ignite cuando esta bajo
                {
                    if (target.Health - ObjectManager.Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite) <= 0)
                        ObjectManager.Player.Spellbook.CastSpell(skills.getIgnite(), target);
                }
                #endregion
            }
            else if (useQ && !useW && useE && useR)
            {
               // Utils.ShowNotification("QWER", System.Drawing.Color.White, 100);
                if (p.getMenu().Item("comboWLH").GetValue<bool>() && ObjectManager.Player.HealthPercent <= vidaMin)
                    skills.wCast(p.getPlayer());

                if (skills.qCast(target, hit))
                    Utility.DelayAction.Add(700, () => skills.qCast2(target)); 
                



                //Pegar un blanco ¡¡¡¡¡¡¡¡¡¡¡¡IMPORTANTE!!!!!!!!!!!!!!!
               
                #region work
                if (skills.eCast(target))
                { //Pegar un blanco ¡¡¡¡¡¡¡¡¡¡¡¡IMPORTANTE!!!!!!!!!!!!!!!
                    skills.eCast(target);
                    items();
                }

                //Aqui solamente entrar cuando mato al enemigo con las skills que tengo activas en ese momento.
                killeableR();
                
                if (skills.getIgnite().IsReady()) // ignite cuando esta bajo
                {
                    if (target.Health - ObjectManager.Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite) <= 0)
                        ObjectManager.Player.Spellbook.CastSpell(skills.getIgnite(), target);
                }
                #endregion
            }
        }

        /*------------------------------------------------WardJump--------------------------------*/
        public bool jump()
        {
            p.getPlayer().IssueOrder(GameObjectOrder.MoveTo, p.getPlayer().Position.Extend(Game.CursorPos, 150));

            IEnumerable<Obj_AI_Minion> Wards = ObjectManager.Get<Obj_AI_Minion>().Where(ward => ward.Distance(Game.CursorPos) < 150 
                && !ward.IsDead);//Wards and minions
            IEnumerable<Obj_AI_Hero> Heros = ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.Distance(Game.CursorPos) < 150
                && !hero.IsDead && !hero.IsMe);

            SecondWTime = Convert.ToInt32(Math.Round(Game.Ping + skills.getW().Instance.SData.SpellTotalTime + 100, 
                MidpointRounding.AwayFromZero));

            #region ward ya existe
            if (skills.getW().IsReady() && Wards.Any() || Heros.Any())
            {
                if (nearestWard(Wards).IsValid)
                {
                    skills.getW().CastOnUnit(nearestWard(Wards));
                    Utility.DelayAction.Add(SecondWTime, () => skills.getW().Cast());
                    LastPlaced = Environment.TickCount;
                    return true;
                }

                if (Heros.FirstOrDefault().IsValid && Heros.FirstOrDefault().IsValidTarget(skills.getW().Range))
                {
                    skills.getW().CastOnUnit(Heros.FirstOrDefault());
                    Utility.DelayAction.Add(SecondWTime, () => skills.getW().Cast());
                    LastPlaced = Environment.TickCount;
                    return true;
                }
            }
            #endregion

            if (skills.getW().IsReady() && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name == "BlindMonkWOne")
            {
                if (Environment.TickCount <= LastPlaced + 3000) return false;
                Vector3 cursorPos = Game.CursorPos;
                Vector3 myPos = p.getPlayer().ServerPosition;

                Vector3 delta = cursorPos - myPos;
                delta.Normalize();

                wardPosition = myPos + delta * (600 - 5);

                InventorySlot invSlot = Items.GetWardSlot();

                if (!invSlot.IsValidSlot()) return false;

                Items.UseItem((int)invSlot.Id, wardPosition);
                LastPlaced = Environment.TickCount;

                Utility.DelayAction.Add(Game.Ping + 100, () => skills.getW().CastOnUnit(nearestWard()));
                Utility.DelayAction.Add(Game.Ping + 100 + SecondWTime, () =>skills.getW().Cast());
                return true;
            }

            return false;
        }
        /*------------------------------------------------nearestWard--------------------------------*/
        public  Obj_AI_Minion nearestWard()
        {
            if (!ObjectManager.Get<Obj_AI_Minion>().Where(ward => ward.IsAlly && ward.Name.ToLower().Contains("ward") 
                && Geometry.Distance(ward.ServerPosition, Game.CursorPos) < 1900 && 
                Geometry.Distance(p.getPlayer().ServerPosition, ward.ServerPosition) <= 
                skills.getW().Range).Any()) return null;

            Dictionary<Obj_AI_Minion, float> Distances = new Dictionary<Obj_AI_Minion, float>();
            Vector3 Mousepos = Game.CursorPos;

            int i = new int();
            i = 0;

            foreach (Obj_AI_Minion Ward in ObjectManager.Get<Obj_AI_Minion>().Where(ward => ward.IsAlly &&
                ward.Name.ToLower().Contains("ward") && Geometry.Distance(ward.ServerPosition, Game.CursorPos) < 1900 && 
                Geometry.Distance(p.getPlayer().ServerPosition, ward.ServerPosition) <= skills.getW().Range))
            {
                if (i != 0 && Geometry.Distance(Ward.ServerPosition, Mousepos) <= Distances.LastOrDefault().Value)
                    //If the new ward is at a shorter distance.
                {
                    Distances.Remove(Distances.LastOrDefault().Key);//Remove the last ward.
                    Distances.Add(Ward, Geometry.Distance(Ward.ServerPosition, Mousepos));
                    //Here, we add the new ward to the dictionary.
                }
                else if (i != 0 && Geometry.Distance(Ward.ServerPosition, Mousepos) >= Distances.LastOrDefault().Value)
                    //If the new ward is at a greater distance. we don't do nothing.
                {

                }
                else//First ward found in the loop.
                {
                    Distances.Add(Ward, Geometry.Distance(Ward.ServerPosition, Mousepos));//As a first ward, we simply add it.
                }

                i += 1;
            }

            if (Distances.LastOrDefault().Key.IsValid) return Distances.LastOrDefault().Key;
            else return null;
        }
        public Obj_AI_Minion nearestWard(IEnumerable<Obj_AI_Minion> Wards)
        {
            if (!Wards.Any()) return null;

            Dictionary<Obj_AI_Minion, float> Distances = new Dictionary<Obj_AI_Minion, float>();
            Vector3 Mousepos = Game.CursorPos;

            int i = new int();
            i = 0;

            foreach (Obj_AI_Minion Ward in Wards)
            {
                if (i != 0 && Geometry.Distance(Ward.ServerPosition, Mousepos) <= Distances.LastOrDefault().Value)
                    //If the new ward is at a shorter distance.
                {
                    Distances.Remove(Distances.LastOrDefault().Key);//Remove the last ward.
                    Distances.Add(Ward, Geometry.Distance(Ward.ServerPosition, Mousepos));
                    //Here, we add the new ward to the dictionary.
                }
                else if (i != 0 && Geometry.Distance(Ward.ServerPosition, Mousepos) >= Distances.LastOrDefault().Value)
                    //If the new ward is at a greater distance. we don't do nothing.
                {

                }
                else//First ward found in the loop.
                {
                    Distances.Add(Ward, Geometry.Distance(Ward.ServerPosition, Mousepos));//As a first ward, we simply add it.
                }

                i += 1;
            }

            if (Distances.LastOrDefault().Key.IsValid) return Distances.LastOrDefault().Key;
            else return null;
        }
        public Obj_AI_Minion NearestWard(IEnumerable<Obj_AI_Minion> wards)
        {
            if (!wards.Any()) return null;

            Dictionary<Obj_AI_Minion, float> Distances = new Dictionary<Obj_AI_Minion, float>();
            Vector3 Mousepos = Game.CursorPos;

            int i = new int();
            i = 0;

            foreach (Obj_AI_Minion Ward in wards)
            {
                if (i != 0 && Geometry.Distance(Ward.ServerPosition, Mousepos) <= Distances.LastOrDefault().Value)//If the new ward is at a shorter distance.
                {
                    Distances.Remove(Distances.LastOrDefault().Key);//Remove the last ward.
                    Distances.Add(Ward, Geometry.Distance(Ward.ServerPosition, Mousepos));//Here, we add the new ward to the dictionary.
                }
                else if (i != 0 && Geometry.Distance(Ward.ServerPosition, Mousepos) >= Distances.LastOrDefault().Value)//If the new ward is at a greater distance. we don't do nothing.
                {

                }
                else//First ward found in the loop.
                {
                    Distances.Add(Ward, Geometry.Distance(Ward.ServerPosition, Mousepos));//As a first ward, we simply add it.
                }

                i += 1;
            }

            if (Distances.LastOrDefault().Key.IsValid) return Distances.LastOrDefault().Key;
            else return null;
        }

        public InventorySlot getBestWardItem()
        {
            InventorySlot ward = Items.GetWardSlot();
            if (ward == default(InventorySlot)) return null;
            return ward;
        }

        /*------------------------------------------------InsecsJump--------------------------------*/
        public bool inDistance(Vector2 pos1, Vector2 pos2, float distance)
        {
            float dist2 = Vector2.DistanceSquared(pos1, pos2);
            return (dist2 <= distance * distance) ? true : false;
        }

        public bool putWard(Vector2 pos) //Corregir
        {
            InventorySlot invSlot = getBestWardItem();
            Items.UseItem((int)invSlot.Id, pos);
            return true;
        }
        public void moveTo(Vector2 Pos)
        {
            p.getPlayer().IssueOrder(GameObjectOrder.MoveTo, Pos.To3D());
        }

        public void insecJump(Vector2 pos)
        {
            Vector2 posStart = pos;
            if (!skills.getW().IsReady())
                return;
            bool wardIs = false;
            if (!inDistance(pos, p.getPlayer().ServerPosition.To2D(), skills.getW().Range + 15))
            {
                pos = p.getPlayer().ServerPosition.To2D() + Vector2.Normalize(pos - p.getPlayer().ServerPosition.To2D()) * 600;
            }

            if (!skills.getW().IsReady() && skills.getW().ChargedSpellName == "")
                return;
            foreach (Obj_AI_Base ally in ObjectManager.Get<Obj_AI_Base>().Where(ally => ally.IsAlly
                && !(ally is Obj_AI_Turret) && inDistance(pos, ally.ServerPosition.To2D(), 200)))
            {
                wardIs = true;
                moveTo(pos);
                if (inDistance(p.getPlayer().ServerPosition.To2D(), ally.ServerPosition.To2D(), skills.getW().Range + 
                    ally.BoundingRadius))
                {
                    skills.getW().Cast(ally);

                }
                return;
            }
            Polygon pol;
            if ((pol = p.getMap().getInWhichPolygon(pos)) != null)
            {
                if (inDistance(pol.getProjOnPolygon(pos), p.getPlayer().ServerPosition.To2D(), skills.getW().Range + 15) && 
                    !wardIs && inDistance(pol.getProjOnPolygon(pos), pos, 200))
                {
                    if (lastWardjump < Environment.TickCount)
                    {
                        putWard(pos);
                        lastWardjump = Environment.TickCount + 1000;
                    }
                }
            }
            else if (!wardIs)
            {
                if (lastWardjump < Environment.TickCount)
                {
                    putWard(pos);
                    lastWardjump = Environment.TickCount + 1000;
                }

            }
        }
        public Vector3 insecpos(Obj_AI_Hero ts)
        {
            return Game.CursorPos.Extend(ts.Position, Game.CursorPos.Distance(ts.Position) + 250);
        }
        public Vector3 insecposTower(Obj_AI_Hero target)
        {
            Obj_AI_Turret turret = ObjectManager.Get<Obj_AI_Turret>().Where(tur => tur.IsAlly && tur.Health > 0 && 
                !tur.IsMe).OrderBy(tur => tur.Distance(p.getPlayer().ServerPosition)).Where(tur => tur.Distance(target.Position) <= 
                                                                                                                        1500).First();
            return target.Position + Vector3.Normalize(turret.Position - target.Position) + 100;

        }
        public Vector3 insecposToAlly(Obj_AI_Hero target, Obj_AI_Hero ally)
        {
            return ally.Position.Extend(target.Position, ally.Position.Distance(target.Position) + 250);
        }
        public Vector3 getward(Obj_AI_Hero target)
        {
            Obj_AI_Turret turret = ObjectManager.Get<Obj_AI_Turret>().Where(tur => tur.IsAlly && tur.Health > 0 && 
                !tur.IsMe).OrderBy(tur => tur.Distance(p.getPlayer().ServerPosition)).First();
            return target.ServerPosition + Vector3.Normalize(turret.ServerPosition - target.ServerPosition) * (-300);
        }

        /*------------------------------------------------Insecs--------------------------------*/
        public void InsecFlashR(Obj_AI_Hero target)
        {
            p.getPlayer().IssueOrder(GameObjectOrder.MoveTo, p.getPlayer().Position.Extend(Game.CursorPos, 150));
            if (skills.getR().IsReady())
                if (insecpos(target).Distance(p.getPlayer().Position) < 375)
                {
                    skills.getR().CastOnUnit(target);
                    Utility.DelayAction.Add(Game.Ping + 125, () => ObjectManager.Player.Spellbook.CastSpell(ObjectManager.Player.GetSpellSlot("SummonerFlash"), insecpos(target)));
                }
        }
        public void updateInsec()
        {
            if (p.getMenu().Item("OrbwalkInsec").GetValue<bool>())
                p.getPlayer().IssueOrder(GameObjectOrder.MoveTo, p.getPlayer().Position.Extend(Game.CursorPos, 150));
            if (!skills.getR().IsReady()) return;
            var target = TargetSelector.GetTarget(1300, TargetSelector.DamageType.Physical);
            insecQMode(target);
        }
        public void updateInsecFlash()
        {
            var target = TargetSelector.GetTarget(1300, TargetSelector.DamageType.Physical);
            InsecFlashR(target);
        }

        public void insecQMode(Obj_AI_Hero target)
        {/*
            if (beforeall == false)
            {
                if (skills.getQ().IsReady() && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name == "BlindMonkQOne")
                {
                    Steps = "One";
                }
                else if (skills.getQ().IsReady() && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name == "blindmonkqtwo")
                {
                    Steps = "Two";
                }
                else
                {
                    Steps = "Three";
                }
                beforeall = true;
            }

            if (target.IsValidTarget(skills.getQ().Range))
            {
                if (Steps == "One") //First hit q
                {
                    if (skills.getW().IsInRange(target.Position + 50))
                    {
                        Steps = "Three";
                    }
                    else
                    {
                        if (skills.getQ().IsReady() && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name == "BlindMonkQOne")
                        {
                            if (skills.getQ().CastIfHitchanceEquals(
                                                target, skills.hitchanceCheck(p.getMenu().Item("seth").GetValue<Slider>().Value)))
                                Steps = "Two";
                        }
                    }
                }
                else if (Steps == "Two") // hit second q
                {
                    if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name == "blindmonkqtwo")
                    {
                        secondQTime = Convert.ToInt32(Math.Round(Game.Ping + 
                                                        skills.getQ().Instance.SData.SpellTotalTime, MidpointRounding.AwayFromZero));
                        if (skills.getQ().Cast())
                        {
                            if (!getBestWardItem().IsValidSlot() && p.getMenu().Item("useflash").GetValue<bool>())
                            {

                                //Steps = "TrickR";
                                Steps = "Flash";
                            }
                            else
                            {
                                Steps = "Three";
                            }
                        }

                    }
                    else
                    {
                        //   Steps = "One";
                    }
                }
                else if (Steps == "Three") // put ward
                {
                    if (p.getPlayer().Distance(getward(target)) <= 600 && skills.getW().IsReady())
                    {
                        Utility.DelayAction.Add(secondQTime, () => insecJump(getInsecPos(target).To2D()));
                        Steps = "Four";
                    }
                }
                else if (Steps == "Four")  //go to the ward
                {
                    Utility.DelayAction.Add(Game.Ping + 100, () => insecJump(getInsecPos(target).To2D()));
                    Utility.DelayAction.Add(Game.Ping + 110, () => fiveornot());
                }
                else if (Steps == "Flash") // hit w
                {
                    if (insecpos(target).Distance(p.getPlayer().Position) < 400)
                    {
                        ObjectManager.Player.Spellbook.CastSpell(ObjectManager.Player.GetSpellSlot("SummonerFlash"),
                            getInsecPos(target));
                        Steps = "Five";
                    }
                }
                else if (Steps == "TrickR") //truco de flash
                {
                    if (insecpos(target).Distance(p.getPlayer().Position) < 375)
                    {
                        if (skills.getR().CastOnUnit(target))
                        {
                            Steps = "TrickFlash";
                        }
                    }
                }
                else if (Steps == "TrickFlash") //truco de flash
                {
                    if (insecpos(target).Distance(p.getPlayer().Position) < 375)
                    {
                        Utility.DelayAction.Add(Game.Ping + 125, () => 
                            ObjectManager.Player.Spellbook.CastSpell(ObjectManager.Player.GetSpellSlot("SummonerFlash"), 
                            getInsecPos(target)));
                        Steps = "One";
                    }
                }
                else if (Steps == "Five" && !skills.getW().Cast()) // and hit the kick
                {
                    Utility.DelayAction.Add(Convert.ToInt32(Math.Round(Game.Ping + skills.getW().Instance.SData.SpellTotalTime, 
                        MidpointRounding.AwayFromZero)), () => skills.getR().CastOnUnit(target));// it dont hit anything
                    Steps = "One";
                }
                else                                  
                    Steps = "One";             
                
            }*/
        }
        public Vector3 getInsecPos(Obj_AI_Hero target)
        {
            if (p.getMenu().Item("Mode").GetValue<StringList>().SelectedIndex == 0)
            {
                return insecposTower(target); // insec torre
                //  Game.PrintChat("");
            }
            else if (p.getMenu().Item("Mode").GetValue<StringList>().SelectedIndex == 1)
            {
                return insecposToAlly(insecEnemy, insecAlly); //insec ally  
            }
            else if (p.getMenu().Item("Mode").GetValue<StringList>().SelectedIndex == 2)
            {
                return insecpos(target); // insec normal
            }
            return insecpos(target);
        }
    }
}