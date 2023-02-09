using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums
{
    public enum TouchInfo
        {
            None = 99,
            Began = 0,
            Moved = 1,
            Stationary = 2,
            Ended = 3,
            Canceled = 4,
        }

        public enum ColorString
        {
            yellow,
            green,
            red
        }


        public enum InAppStatus
        {
            NotAvailable,
            Owned,
            NotOwned
        }

        public enum ActionClick
        {
            None = 0,
            Play = 1,
            Rate = 2,
            Share = 3,
            Policy = 4,
            Feedback = 5,
            Term = 6,
            Energy = 7,
            Coin = 8,
            Gem = 9,
            NoAds = 10,
            Settings = 11,
            Quest = 12,
            DailyQuest = 13
        }

        public enum Reason
        {
            DailyReward = 0,
            InApp = 1,
            Ads = 2,
            QuestReward = 3,
            DailyQuestReward = 4,
            DailyLogin = 5
        }

        public enum TypeItem
        {
            Coin = 0,
            Gem = 1,
            Energy = 2,
            RemoveADS = 3
        }

        public enum TypePackIAP
        {
            GEM_1 = 0,
            GEM_2 = 1,
            GEM_3 = 3,
            GEM_4 = 4,
            ROMOVE_ADS = 5,
        }

        public enum ActionWatchVideo
        {
            Replay = 0,
            AddGem_InShop = 1,
            X2Coin_EndGame = 2,
            Quest_WacthVideo = 3,
            Upgrade_Player = 4,
            AddCoin_InUpgradePlayer = 5,
            BuyShop = 6,
            DailyLogin = 7,
            Popup_Show_Ads = 8,
            VideoGirl = 9,
            BuyItemPlayer = 10,
            BuyItemPlayerRandom = 11,
            X2VQMM = 12,
            OnlineReward = 13,
            SpinADS = 14,
            RewardAccumulate = 15,
            RewardAccumulateX2 = 16,
            BuyItemEndless = 17,
        }

        public enum QuestEnum
        {
            Kill_X_Enemy = 0,
            Complete_X_Level = 1,
            Watch_X_Video = 2,
            Finish_With_Full_Health = 3,
            Pass_Level_With_No_Damage = 4,
            Spent_X_Gem = 5,
            Unlock_New_X_Character = 6
        }

        public enum TypeChest
        {
            Common = 0,
            Rare = 1,
            Epic = 2
        }

        public enum QuestStatus
        {
            Doing = 0,
            Finished = 1,
            Claimed = 2
        }
}
