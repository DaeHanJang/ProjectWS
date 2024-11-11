namespace WhiteSurvivor {
    //Error
    public enum ErrorStatus{
        None,
        FirebaseError,
        GPGSLoginFailed,
        FirebaseLoginFailed
    };

    //Warning
    public enum WarningStatus {
        None,
        GuestLogin,
        SignInOnProgress,
        LoggingIn,
        DontLogin
    };
}