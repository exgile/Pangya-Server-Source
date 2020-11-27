using PangyaAPI.IFF.Collections;
using PangyaAPI.IFF.Flags;
using PangyaAPI.IFF.Tools;
using PangyaAPI.ZIP.Tools;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace PangyaAPI.IFF.Manager
{
    public class IFFFile
    {
        #region Fields

        string FileName;
        /// <summary>
        /// Read data from the Part.iff file
        /// </summary>
        public PartCollection Part { get; set; }

        /// <summary>
        /// Read data from the Card.iff file
        /// </summary>
        public CardCollection Card { get; set; }

        /// <summary>
        /// Read data from the Part.iff file
        /// </summary>
        public CaddieCollection Caddie { get; set; }

        /// <summary>
        /// Read data from the Item.iff file
        /// </summary>
        public ItemCollection Item { get; set; }

        /// <summary>
        /// Read data from the LevelUpPrizeItem.iff file
        /// </summary>
        public LevelUpPrizeItemCollection LevelUpPrizeItem { get; set; }

        /// <summary>
        /// Read data from the Character.iff file
        /// </summary>
        public CharacterCollection Character { get; set; }

        /// <summary>
        /// Read data from the Ball.iff file
        /// </summary>
        public BallCollection Ball { get; set; }

        /// <summary>
        /// Read data from the Ability.iff file
        /// </summary>
        public AbilityCollection Ability { get; set; }

        /// <summary>
        /// Read data from the Skin.iff file
        /// </summary>
        public SkinCollection Skin { get; set; }

        /// <summary>
        /// Read data from the CaddieItem.iff file
        /// </summary>
        public CaddieItemCollection CaddieItem { get; set; }

        /// <summary>
        /// Read data from the Club.iff file
        /// </summary>
        public ClubCollection Club { get; set; }

        /// <summary>
        /// Read data from the ClubSet.iff file
        /// </summary>
        public ClubSetCollection ClubSet { get; set; }

        /// <summary>
        /// Read data from the Course.iff file
        /// </summary>
        public CourseCollection Course { get; set; }

        /// <summary>
        /// Read data from the CutinInformation.iff file
        /// </summary>
        public CutinInformationCollection CutinInformation { get; set; }

        /// <summary>
        /// Read data from the Desc.iff file
        /// </summary>
        public DescCollection Desc { get; set; }

        /// <summary>
        /// Read data from the Furniture.iff file
        /// </summary>
        public FurnitureCollection Furniture { get; set; }

        /// <summary>
        /// Read data from the FurnitureAbility.iff file
        /// </summary>
        public FurnitureAbilityCollection FurnitureAbility { get; set; }

        /// <summary>
        /// Read data from the Mascot.iff file
        /// </summary>
        public MascotCollection Mascot { get; set; }

        /// <summary>
        /// Read data from the TikiSpecialTable.iff file
        /// </summary>
        public TikiSpecialTableCollection TikiSpecialTable { get; set; }

        /// <summary>
        /// Read data from the TikiRecipe.iff file
        /// </summary>
        public TikiRecipeCollection TikiRecipe { get; set; }

        /// <summary>
        /// Read data from the TikiPointTable.iff file
        /// </summary>
        public TikiPointTableCollection TikiPointTable { get; set; }

        /// <summary>
        /// Read data from the CadieMagicBox.iff file
        /// </summary>
        public CadieMagicBoxCollection CadieMagicBox { get; set; }

        /// <summary>
        /// Read data from the CadieMagicBoxRandom.iff file
        /// </summary>
        public CadieMagicBoxRandomCollection CadieMagicBoxRandom { get; set; }

        /// <summary>
        /// Read data from the HairStyle.iff file
        /// </summary>
        public HairStyleCollection HairStyle { get; set; }

        /// <summary>
        /// Read data from the Match.iff file
        /// </summary>
        public MatchCollection Match { get; set; }

        /// <summary>
        /// Read data from the SetItem.iff file
        /// </summary>
        public SetItemCollection SetItem { get; set; }

        /// <summary>
        /// Read data from the Enchant.iff file
        /// </summary>
        public EnchantCollection Enchant { get; set; }

        /// <summary>
        /// Read data from the Achievement.iff file
        /// </summary>
        public AchievementCollection Achievement { get; set; }

        /// <summary>
        /// Read data from the QuestStuff.iff file
        /// </summary>
        public QuestStuffCollection QuestStuff { get; set; }

        /// <summary>
        /// Read data from the QuestItem.iff file
        /// </summary>
        public QuestItemCollection QuestItem { get; set; }

        /// <summary>
        /// Read data from the SetEffectTable.iff file
        /// </summary>
        public SetEffectTableCollection SetEffectTable { get; set; }

        /// <summary>
        /// Read data from the AuxPart.iff file
        /// </summary>
       public AuxPartCollection AuxPart { get; set; }
        #endregion

        #region Constructor

        public IFFFile(string filename)
        {
            FileName = filename;
            Part = new PartCollection();
            Card = new CardCollection();
            Caddie = new CaddieCollection();
            Item = new ItemCollection();
            LevelUpPrizeItem = new LevelUpPrizeItemCollection();
            Character = new CharacterCollection();
            Ball = new BallCollection();
            Ability = new AbilityCollection();
            Skin = new SkinCollection();
            CaddieItem = new CaddieItemCollection();
            Club = new ClubCollection();
            ClubSet = new ClubSetCollection();
            Course = new CourseCollection();
            CutinInformation = new CutinInformationCollection();
            Desc = new DescCollection();
            Furniture = new FurnitureCollection();
            FurnitureAbility = new FurnitureAbilityCollection();
            Mascot = new MascotCollection();
            TikiSpecialTable = new TikiSpecialTableCollection();
            TikiRecipe = new TikiRecipeCollection();
            TikiPointTable = new TikiPointTableCollection();
            CadieMagicBox = new CadieMagicBoxCollection();
            CadieMagicBoxRandom = new CadieMagicBoxRandomCollection();
            HairStyle = new HairStyleCollection();
            Match = new MatchCollection();
            SetItem = new SetItemCollection();
            Enchant = new EnchantCollection();
            Achievement = new AchievementCollection();
            AuxPart = new AuxPartCollection();
        }

        public IFFFile()
        {
            FileName = "data/pangya_gb.iff";
            Part = new PartCollection();
            Card = new CardCollection();
            Caddie = new CaddieCollection();
            Item = new ItemCollection();
            LevelUpPrizeItem = new LevelUpPrizeItemCollection();
            Character = new CharacterCollection();
            Ball = new BallCollection();
            Ability = new AbilityCollection();
            Skin = new SkinCollection();
            CaddieItem = new CaddieItemCollection();
            Club = new ClubCollection();
            ClubSet = new ClubSetCollection();
            Course = new CourseCollection();
            CutinInformation = new CutinInformationCollection();
            Desc = new DescCollection();
            Furniture = new FurnitureCollection();
            FurnitureAbility = new FurnitureAbilityCollection();
            Mascot = new MascotCollection();
            TikiSpecialTable = new TikiSpecialTableCollection();
            TikiRecipe = new TikiRecipeCollection();
            TikiPointTable = new TikiPointTableCollection();
            CadieMagicBox = new CadieMagicBoxCollection();
            CadieMagicBoxRandom = new CadieMagicBoxRandomCollection();
            HairStyle = new HairStyleCollection();
            Match = new MatchCollection();
            SetItem = new SetItemCollection();
            Enchant = new EnchantCollection();
            Achievement = new AchievementCollection();
            QuestStuff = new QuestStuffCollection();
            QuestItem = new QuestItemCollection();
            SetEffectTable = new SetEffectTableCollection();
            AuxPart = new AuxPartCollection();
            LoadIff();
        }

        #endregion

        #region Methods 

        #region Private

        #endregion

        #region Public

        public void LoadIff()
        {
            bool result;
            try
            {
                if (Directory.Exists("data") && File.Exists(FileName))
                {
                    var zip = ZipFileEx.Open(FileName);

                    result = Part.Load(zip.GetFileData("Part.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.Part]: Items Loads {Part.Count}");
                    }

                    Part.GetItem(134258753);

                    result = Card.Load(zip.GetFileData("Card.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.Card]: Items Loads {Card.Count}");
                    }

                    result = Caddie.Load(zip.GetFileData("Caddie.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.Caddie]: Items Loads {Caddie.Count}");
                    }

                    result = Item.Load(zip.GetFileData("Item.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.Item]: Items Loads {Item.Count}");
                    }

                    result = LevelUpPrizeItem.Load(zip.GetFileData("LevelUpPrizeItem.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.LevelUpPrizeItem]: Items Loads {LevelUpPrizeItem.Count}");
                    }

                    result = Character.Load(zip.GetFileData("Character.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.Character]: Items Loads {Character.Count}");
                    }

                    result = Ball.Load(zip.GetFileData("Ball.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.Ball]: Items Loads {Ball.Count}");
                    }

                    result = Ability.Load(zip.GetFileData("Ability.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.Ability]: Items Loads {Ability.Count}");
                    }

                    result = Skin.Load(zip.GetFileData("Skin.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.Skin]: Items Loads {Skin.Count}");
                    }
                    result = CaddieItem.Load(zip.GetFileData("CaddieItem.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.CaddieItem]: Items Loads {CaddieItem.Count}");
                    }

                    result = Club.Load(zip.GetFileData("Club.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.Club]: Items Loads {Club.Count}");
                    }

                    result = ClubSet.Load(zip.GetFileData("ClubSet.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.ClubSet]: Items Loads {ClubSet.Count}");
                    }

                    result = Course.Load(zip.GetFileData("Course.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.Course]: Items Loads {Course.Count}");
                    }

                    result = CutinInformation.Load(zip.GetFileData("CutinInfomation.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.CutinInformation]: Items Loads {CutinInformation.Count}");
                    }

                    result = Desc.Load(zip.GetFileData("Desc.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.Desc]: Items Loads {Desc.Count}");
                    }

                    result = Furniture.Load(zip.GetFileData("Furniture.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.Furniture]: Items Loads {Furniture.Count}");
                    }

                    result = FurnitureAbility.Load(zip.GetFileData("FurnitureAbility.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.FurnitureAbility]: Items Loads {FurnitureAbility.Count}");
                    }

                    result = Mascot.Load(zip.GetFileData("Mascot.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.Mascot]: Items Loads {Mascot.Count}");
                    }

                    result = TikiSpecialTable.Load(zip.GetFileData("TikiSpecialTable.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.TikiSpecialTable]: Items Loads {TikiSpecialTable.Count}");
                    }

                    result = TikiRecipe.Load(zip.GetFileData("TikiRecipe.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.TikiRecipe]: Items Loads {TikiRecipe.Count}");
                    }

                    result = TikiPointTable.Load(zip.GetFileData("TikiPointTable.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.TikiPointTable]: Items Loads {TikiPointTable.Count}");
                    }

                    result = CadieMagicBox.Load(zip.GetFileData("CadieMagicBox.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.CadieMagicBox]: Items Loads {CadieMagicBox.Count}");
                    }

                    result = CadieMagicBoxRandom.Load(zip.GetFileData("CadieMagicBoxRandom.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.CadieMagicBoxRandom]: Items Loads {CadieMagicBoxRandom.Count}");
                    }

                    result = HairStyle.Load(zip.GetFileData("HairStyle.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.HairStyle]: Items Loads {HairStyle.Count}");
                    }

                    result = Match.Load(zip.GetFileData("Match.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.Match]: Items Loads {Match.Count}");
                    }

                    result = SetItem.Load(zip.GetFileData("SetItem.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.SetItem]: Items Loads {SetItem.Count}");
                    }

                    result = Enchant.Load(zip.GetFileData("Enchant.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.Enchant]: Items Loads {Enchant.Count}");
                    }

                    result = Achievement.Load(zip.GetFileData("Achievement.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.Achievement]: Items Loads {Achievement.Count}");
                    }

                    result = QuestItem.Load(zip.GetFileData("QuestItem.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.QuestItem]: Items Loads {QuestItem.Count}");
                    }

                    result = QuestStuff.Load(zip.GetFileData("QuestStuff.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.QuestStuff]: Items Loads {QuestStuff.Count}");
                    }

                    result = SetEffectTable.Load(zip.GetFileData("SetEffectTable.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.SetEffectTable]: Items Loads {SetEffectTable.Count}");
                    }

                    result = AuxPart.Load(zip.GetFileData("AuxPart.iff"));
                    if (result)
                    {
                        Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + $"[Pang.IFF.AuxPart]: Items Loads {AuxPart.Count}");
                    }
                }
                else
                {
                    throw new Exception(" Failed to attempt to load data from file not found: " + FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Pang.IFF.Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Rewriter Pangya_GB.iff
        /// </summary>
        public void Rebuilding()
        {
            if (this.Part.Update)
            { }
            if (this.Card.Update)
            { }
        }

        /// <summary>
        /// Get Item Name by TypeID
        /// </summary>   
        public string GetName(uint TypeID)
        {
            switch (TypeID.GetItemGroup())
            {
                case IffGroupFlag.ITEM_TYPE_CHARACTER:
                    return Character.GetItemName(TypeID);

                case IffGroupFlag.ITEM_TYPE_PART:
                    //Part
                    return Part.GetItemName(TypeID);
                case IffGroupFlag.ITEM_TYPE_HAIR_STYLE:
                    {
                        return HairStyle.GetItemName(TypeID);
                    }
                case IffGroupFlag.ITEM_TYPE_CLUB:
                    return ClubSet.GetItemName(TypeID);

                case IffGroupFlag.ITEM_TYPE_BALL:
                    // Ball
                    return Ball.GetItemName(TypeID);

                case IffGroupFlag.ITEM_TYPE_USE:
                    // Normal Item
                    return Item.GetItemName(TypeID);

                case IffGroupFlag.ITEM_TYPE_CADDIE:
                    // Cadie
                    return Caddie.GetItemName(TypeID);

                case IffGroupFlag.ITEM_TYPE_CADDIE_ITEM:
                    return CaddieItem.GetItemName(TypeID);

                case IffGroupFlag.ITEM_TYPE_SETITEM:
                    // Part
                    return SetItem.GetItemName(TypeID);

                case IffGroupFlag.ITEM_TYPE_SKIN:
                    return Skin.GetItemName(TypeID);

                case IffGroupFlag.ITEM_TYPE_MASCOT:
                    return Mascot.GetItemName(TypeID);

                case IffGroupFlag.ITEM_TYPE_CARD:
                    return Card.GetItemName(TypeID);

                case IffGroupFlag.ITEM_TYPE_AUX:
                    return AuxPart.GetItemName(TypeID);

            }
            return "Unknown Item Name";
        }

        public bool IsSelfDesign(uint itemTypeID)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
    }
}
