namespace WhiteSurvivor {
    //Error
    public enum ErrorState {
        None,
        FirebaseInitializationError,
        GPGSAuthenticationFailure,
        SignInWithCredentialAsyncCanceled,
        SignInWithCredentialAsyncError,
        SignInAnonymouslyAsyncCanceled,
        SignInAnonymouslyAsyncError
    };

    //Warning
    public enum WarningState {
        None,
        SignInOnProgress,
        LoggedIn,
        GuestLogin,
        Logout,
        NotLogin
    };
}
