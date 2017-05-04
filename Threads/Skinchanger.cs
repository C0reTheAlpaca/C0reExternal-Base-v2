using System;
using System.Threading;
using C0reExternalBase_v2.Utility;
using C0reExternalBase_v2.Variables;
using static C0reExternalBase_v2.Memory;
using static C0reExternalBase_v2.Structs.Entitys;
using static C0reExternalBase_v2.Structs.CheatMenu;

namespace C0reExternalBase_v2.Threads
{
    class Skinchanger
    {
        private static bool bWasActive = false;

        public static void Reskin()
        {
            while (true)
            {
                if (Settings.m_bSkinchanger)
                {
                    // Save That We Used The Skinchanger
                    bWasActive = true;

                    int OverrideTexture;

                    for (var i = 1; i < 16; ++i)
                    {
                        // Create A New Weapon Object
                        Weapon Weapon = new Weapon();

                        // Get Adress To Current Weapon Out Of Array Of Currently Equipped Weapons
                        int iCurWeaponAdress = (ManageMemory.ReadMemory<int>(LocalPlayer.m_iBase + Offsets.m_hMyWeapons + (i - 1) * 0x4)) & 0xFFF;

                        // Get Baseadress of The Current Weapon
                        Weapon.m_iBase = ManageMemory.ReadMemory<int>(Offsets.m_ClientPointer + Offsets.m_dwEntityList + (iCurWeaponAdress - 1) * 0x10);

                        // Get The WeaponID
                        Weapon.m_iItemDefinitionIndex = ManageMemory.ReadMemory<int>(Weapon.m_iBase + Offsets.m_iItemDefinitionIndex);

                        // Get EntityID Of WeaponOwner
                        Weapon.m_iXuIDLow = ManageMemory.ReadMemory<int>(Weapon.m_iBase + Offsets.m_OriginalOwnerXuidLow);

                        // Get Weapon Skin
                        Weapon.m_iTexture = ManageMemory.ReadMemory<int>(Weapon.m_iBase + Offsets.m_nFallbackPaintKit);


                        // Define Which Skin We Want (Based On The Current Weapon)
                        switch (Weapon.m_iItemDefinitionIndex)
                        {
                            case (int)ItemDefinition.WEAPON_AK47:
                                OverrideTexture = 44;
                                break;
                            case (int)ItemDefinition.WEAPON_M4A1_SILENCER:
                                OverrideTexture = 440;
                                break;
                            case (int)ItemDefinition.WEAPON_M4A1:
                                OverrideTexture = 309;
                                break;
                            case (int)ItemDefinition.WEAPON_AWP:
                                OverrideTexture = 344;
                                break;
                            case (int)ItemDefinition.WEAPON_GLOCK:
                                OverrideTexture = 38;
                                break;
                            case (int)ItemDefinition.WEAPON_HKP2000:
                                OverrideTexture = 389;
                                break;
                            case (int)ItemDefinition.WEAPON_USP_SILENCER:
                                OverrideTexture = 290;
                                break;
                            case (int)ItemDefinition.WEAPON_UMP45:
                                OverrideTexture = 556;
                                break;
                            case (int)ItemDefinition.WEAPON_P250:
                                OverrideTexture = 102;
                                break;
                            case (int)ItemDefinition.WEAPON_FIVESEVEN:
                                OverrideTexture = 427;
                                break;
                            case (int)ItemDefinition.WEAPON_DEAGLE:
                                OverrideTexture = 231;
                                break;
                            case (int)ItemDefinition.WEAPON_TEC9:
                                OverrideTexture = 599;
                                break;
                            default:
                                OverrideTexture = 1337;
                                break;
                        }

                        // Check If The Weapon Doesn't Have The Skin Yet
                        if (Weapon.m_iTexture != OverrideTexture && OverrideTexture != 1337)
                        {
                            // Set New Item Values
                            ManageMemory.WriteMemory<int>(Weapon.m_iBase + Offsets.m_iItemIDLow, -1);
                            ManageMemory.WriteMemory<int>(Weapon.m_iBase + Offsets.m_nFallbackPaintKit, OverrideTexture);
                            ManageMemory.WriteMemory<int>(Weapon.m_iBase + Offsets.m_nFallbackSeed, 661);
                            ManageMemory.WriteMemory<int>(Weapon.m_iBase + Offsets.m_nFallbackStatTrak, 1337);
                            ManageMemory.WriteMemory<float>(Weapon.m_iBase + Offsets.m_flFallbackWear, 0.00001f);
                            ManageMemory.WriteMemory<int>(Weapon.m_iBase + Offsets.m_iAccountID, Weapon.m_iXuIDLow);
                            ManageMemory.WriteMemory<char[]>(Weapon.m_iBase + Offsets.m_szCustomName, "C0reExternal".ToCharArray());

                            // Force Textures To Reload
                            ManageMemory.WriteMemory<int>(LocalPlayer.m_iClientState + 0x16C, -1);
                        }
                    }
                }
                else if (!Settings.m_bSkinchanger && bWasActive)
                {
                    // If Skinchanger Was Active & Is Now Inactive Force Textures To Reload
                    ManageMemory.WriteMemory<int>(LocalPlayer.m_iClientState + 0x16C, -1);

                    bWasActive = false;
                }
                // (-.-)Zzz...
                Thread.Sleep(5);
            }
        }
    }
}