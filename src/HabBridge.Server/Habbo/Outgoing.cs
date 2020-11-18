﻿// ReSharper disable InconsistentNaming
namespace HabBridge.Server.Habbo
{
    public enum Outgoing
    {
        Unknown = 0,
        LandingWidget,
        LandingCommunityChallenge,
        LandingPromos,
        LandingReward,
        SendCampaignBadge,
        HotelViewCountdown,
        HotelViewDailyquest,
        HotelViewHallOfFame,
        CompetitionEntrySubmitResult,
        CompetitionVotingInfo,
        SubscriptionStatus,
        UserClubRights, // Replace with UserRights??
        EnableTrading,
        MinimailCount,
        UserProfile,
        LoadVolume,
        LoadWardrobe,
        ReceiveBadge,
        LoveLockDialogue,
        LoveLockDialogueSetLocked,
        LoveLockDialogueClose,
        NewbieStatus,
        IdentityAccounts,
        OfficialRooms,
        NavigatorListings,
        CanCreateRoom,
        PopularRoomTags,
        NavigatorNewFlatCategories,
        NavigatorLiftedRoomsComposer,
        NavigatorMetaDataComposer,
        NavigatorSavedSearchesComposer,
        NavigatorCategorys,
        NewNavigatorSize,
        TargetedOffer,
        CatalogueIndex,
        CataloguePage,
        CatalogueClubPage,
        CatalogOffer,
        CatalogueOfferConfig,
        PurchaseOK,
        CatalogPurchaseNotAllowed,
        CatalogLimitedItemSoldOut,
        SellablePetBreeds,
        ReloadEcotron,
        GiftWrappingConfiguration,
        RecyclerRewards,
        CatalogPromotionGetRooms,
        PublishShop,
        RecyclingState,
        GiftError,
        RetrieveSongID,
        JukeboxNowPlaying,
        JukeboxPlaylist,
        SongsLibrary,
        Songs,
        VoucherError,
        VoucherValid,
        CatalogPromotionGetCategories,
        BuildersClubUpdateFurniCount,
        LoadInventory,
        UpdateInventory,
        AvatarEffect,
        NewInventoryObject,
        RemoveInventoryObject,
        LoadBadgesWidget,
        BotInventory,
        PetInventory,
        AddEffectToInventory,
        ApplyEffect,
        EnableEffect,
        StopAvatarEffect,
        InitialRoomInfo,
        HasOwnerRights,
        SetRoomUser,
        UpdateUserStatus,
        RoomChatOptions,
        RoomError,
        RoomData,
        RoomEnterError,
        RoomFloorItems,
        RoomFloorWallLevels,
        RoomGroup,
        RoomOwnership,
        RoomRightsLevel,
        LoadRoomRightsList,
        RoomBannedList,
        RoomLoadFilter,
        RoomForward,
        GroupRoom,
        OnCreateRoomInfo,
        RoomUpdate,
        RoomWallItems,
        Doorbell,
        DoorbellNoOne,
        DoorbellOpened,
        RoomMuteStatus,
        GetFloorPlanUsedCoords,
        SetFloorPlanDoor,
        RoomsQueue,
        SpectatorMode,
        RoomUserAction,
        RoomUserIdle,
        OutOfRoom,
        ApplyHanditem,
        TypingStatus,
        DanceStatus,
        UpdateUserData,
        UpdateUsername,
        UserLeftRoom,
        UserTags,
        FloodFilter,
        NameChangedUpdates,
        NotAcceptingRequests,
        GiveRoomRights,
        RemoveRights,
        RoomUnbanUser,
        GiveRespects,
        UpdateAvatarAspect,
        UserUpdateNameInRoom,
        UserIsPlayingFreeze,
        UpdateFreezeLives,
        UpdateIgnoreStatus,
        TradeAccept,
        TradeClose,
        TradeCompleted,
        TradeConfirmation,
        TradeStart,
        TradeUpdate,
        AddFloorItem,
        AddWallItem,
        PickUpFloorItem,
        PickUpWallItem,
        DimmerData,
        ItemAnimation,
        UpdateFloorItemExtraData,
        UpdateRoomItem,
        UpdateRoomWallItem,
        UpdateFurniStackMap,
        UpdateTileStackMagicHeight,
        LoadPostIt,
        OpenGift,
        YouTubeLoadPlaylists,
        YouTubeLoadVideo,
        RemovePetFromInventoryComposer,
        WiredRewardAlert,
        SaveWired,
        WiredCondition,
        WiredEffect,
        WiredTrigger,
        BotSpeechList,
        PetInfo,
        PetTrainerPanel,
        AddPetExperience,
        CheckPetName,
        SerializePet,
        RespectPet,
        PetRespectNotification,
        NotifyNewPetLevel,
        SendMonsterplantId,
        PetBreedError,
        PetBreed,
        PetBreedResultError,
        PetBreedResult,
        PlacePetError,
        GeneralErrorHabbo,
        LoadModerationTool,
        ModerationActionResult,
        ModerationToolIssueChatlog,
        ModerationRoomTool,
        ModerationTicketResponse,
        ModerationToolIssue,
        ModerationToolRoomVisits,
        ModerationToolUpdateIssue,
        ModerationToolUserChatlog,
        ModerationToolUserTool,
        ModerationToolRoomChatlog,
        TicketUserAlert,
        AlertNotification,
        SuperNotification,
        EpicPopup,
        CustomUserNotification,
        UsersClassification,
        OpenBullyReport,
        BullyReportSent,
        OpenHelpTool,
        HelperToolConfiguration,
        OnGuideSessionStarted,
        OnGuideSessionPartnerIsTyping,
        OnGuideSessionMsg,
        OnGuideSessionInvitedToGuideRoom,
        OnGuideSessionAttached,
        OnGuideSessionDetached,
        OnGuideSessionError,
        AchievementList,
        AchievementProgress,
        UnlockAchievement,
        QuestAborted,
        QuestCompleted,
        QuestList,
        QuestStarted,
        CitizenshipStatus,
        TalentLevelUp,
        TalentsTrack,
        ConsoleChatError,
        ConsoleChat,
        ConsoleInvitation,
        ConsoleSearchFriend,
        ConsoleSendFriendRequest,
        LoadFriendsCategories,
        FollowFriendError,
        FriendUpdate,
        FindMoreFriendsSuccess,
        ChangeFavouriteGroup,
        FavouriteGroup,
        GroupDataEdit,
        GroupData,
        GroupFurniturePage,
        GroupMembers,
        GroupPurchasePage,
        GroupPurchaseParts,
        GroupAreYouSure,
        GroupConfirmLeave,
        GroupRequestReload,
        GroupForumListings,
        GroupForumData,
        GroupForumThreadRoot,
        GroupForumThreadUpdate,
        GroupForumNewThread,
        GroupForumNewResponse,
        GroupForumReadThread,
        GroupDeleted,
        GameCenterGamesList,
        GameCenterGameAchievements,
        GameCenterLeaderboard,
        GameCenterLeaderboard2,
        GameCenterGamesLeft,
        GameCenterPreviousWinner,
        GameCenterProducts,
        GameCenterAllAchievements,
        GameCenterEnterInGame,
        GameCenterJoinGameQueue,
        GameCenterLoadGameUrl,
        PollQuestions,
        SuggestPoll,
        MatchingPoll,
        MatchingPollAnswered,
        MatchingPollResult,
        EnableNotifications,
        NuxSuggestFreeGifts,
        NuxListGifts,
        SetCameraPrice,
        GenericError,
        InternalLink,
        CameraStorageUrl,
        CameraPurchaseOk,
        ThumbnailSuccess,
        BullyRequestOpened,
        BullyRequestChatlogs,
        BullyRequestVotes,
        BullyRequestResult,

        // Handshake
        InitCrypto,
        SecretKey,
        AuthenticationOk,
        UniqueMachineId,
        DisconnectReason,
        UserObject,
        UserRights,
        Ping,
        
        // Availability
        AvailabilityStatus,

        // CallForHelp
        CfhTopicsInit,

        // Competition
        CurrentTimingCode,

        // Catalog
        BuildersClubSubscriptionStatus,
        BundleDiscountRuleset,
        CatalogIndex,
        CatalogPage,

        // Friendlist
        MessengerInit,
        FriendsListFragment,
        HabboSearchResult,
        NewFriendRequest,
        FriendListUpdate,
        FriendRequests,
        NewConsoleMessage,

        // Inventory.Achievements
        Achievement,
        AchievementPoints,

        // Inventory.AvatarEffect
        AvatarEffects,
        
        // Inventory.Badges
        Badges,
        BadgePointLimits,

        // Inventory.Clothing
        FigureSetIds,

        // Inventory.Purse
        CreditsBalance,

        // Navigator
        UserFlatCats,
        GuestRoomSearchResult,
        Favourites,
        FavouriteChanged,
        RoomRating,
        RoomEvent,
        GetGuestRoomResult,
        NavigatorSettings,

        // Notifications
        HabboBroadcast,
        HabboActivityPointNotification,
        MOTDNotification,
        ActivityPoints,

        // Perk
        PerkAllowances,
        
        // Preferences
        AccountPreferences,
        
        // Quest
        CommunityGoalHallOfFame,

        // RoomSettings
        RoomSettingsData,
        RoomSettingsSaved,

        // Room.Action
        Sleep,

        // Room.Chat
        UserTyping,
        Chat,
        Shout,
        Whisper,

        // Room.Engine
        RoomProperty,
        RoomEntryInfo,
        RoomVisualizationSettings,
        HeightMap,
        FloorHeightMap,
        FurnitureAliases,
        Users,
        UserChange,
        UserUpdate,
        UserRemove,
        Objects,
        ObjectUpdate,
        Items,

        // Room.Permissions
        YouAreOwner,
        YouAreController,
        YouAreNotController,

        // Room.Session
        OpenConnection,
        CloseConnection,
        RoomReady,

        // Tracking
        LatencyPingResponse,

        // Users
        ExtendedProfile,
        HabboUserBadges,
        RelationshipStatusInfo,
        ScrSendUserInfo,
    }
}
