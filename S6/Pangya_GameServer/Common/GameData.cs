using PangyaAPI.PangyaClient.Data;
using PangyaAPI.Tools;
namespace Pangya_GameServer.Common
{
    public struct GameData
    {
        #region Field

        public uint ConnectionID;
        public uint UID;
        public byte GameSlot;
        public byte Role;
        public bool GameReady;
        public VersusInfo Versus;
        public uint Playing;
        public bool GameCompleted;
        public Point3D HolePos3D;
        public uint HolePos;
        public Action Action;
        public GameScoreData ScoreData;
        public bool MyTurn;

        #endregion

        #region Methods

        public void AddWalk(Point3D Pos)
        {
            Action.Vector += Pos;
        }

        public void SetDefault()
        {
            this.GameSlot = 1;
            this.Role = 0;
            this.GameReady = false;
            this.HolePos = 0;
            this.GameCompleted = false;
            this.ConnectionID = 0;
            this.UID = 0;
            this.HolePos3D = new Point3D();
            this.Versus = new VersusInfo();
            ScoreData.Initial();
            this.Action = new Action().Clear();
            this.Versus.LoadAnimation = false;
            this.Versus.ShotSync = false;
            this.Versus.HoleDistance = 0;
            this.Versus.LastHit = 0;
            this.Versus.LastScore = 0;
        }

        public void UpdateScore(bool Sucess)
        {
            ushort S;

            if (!Sucess)
            {
                S = 5;
            }
            else
            {
                S = (ushort)(ScoreData.ShotCount - ScoreData.ParCount);
            }
            Versus.LastScore = (sbyte)S;
            ScoreData.Score = (sbyte)(ScoreData.Score + S);
        }

        #endregion

       
    }
}
