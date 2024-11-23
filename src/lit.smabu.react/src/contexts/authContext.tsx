import React, { createContext, useContext, useEffect, useState, ReactNode } from 'react';
import { PublicClientApplication, AccountInfo, EventMessage, EventType, AuthenticationResult } from '@azure/msal-browser';
import { useMsal } from '@azure/msal-react';
import { msalConfig, loginRequest } from "../configs/authConfig.ts";
import { useNavigate } from 'react-router-dom';

export const msalInstance = new PublicClientApplication(msalConfig);

msalInstance.initialize().then(() => {
    const accounts = msalInstance.getAllAccounts();
    if (accounts.length > 0) {
        msalInstance.setActiveAccount(accounts[0]);
    }

    msalInstance.addEventCallback((event: EventMessage) => {
        if (event.eventType === EventType.LOGIN_SUCCESS && event.payload) {
            const payload = event.payload as AuthenticationResult;
            const account = payload.account;
            msalInstance.setActiveAccount(account);
        }
    });
});

interface AuthContextType {
    account: AccountInfo | null;
    accessToken: string | null;
    isAuthenticated: boolean;
    login: () => Promise<void>;
    logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

const LOCAL_STORAGE_KEYS = {
    ACCESS_TOKEN: 'auth_accesstoken',
    ID_TOKEN: 'auth_idtoken',
    USERNAME: 'auth_username',
};

const setLocalStorageItem = (key: string, value: string) => {
    localStorage.setItem(key, value);
};

const removeLocalStorageItem = (key: string) => {
    localStorage.removeItem(key);
};

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [account, setAccount] = useState<AccountInfo | null>(null);
    const [accessToken, setAccessToken] = useState<string | null>(null);
    const { instance } = useMsal();
    const navigate = useNavigate();

    useEffect(() => {        
        const account = instance.getActiveAccount();
        if (account) {
            setAccount(account);
            instance.acquireTokenSilent(loginRequest).then(response => {
                setAccount(response.account);
                setAccessToken(response.accessToken);
                setIsAuthenticated(true);
                setLocalStorageItem(LOCAL_STORAGE_KEYS.ACCESS_TOKEN, response.accessToken);
                setLocalStorageItem(LOCAL_STORAGE_KEYS.ID_TOKEN, response.idToken);
                setLocalStorageItem(LOCAL_STORAGE_KEYS.USERNAME, response.account.username);
            });
        }
    }, [instance]);

    const login = async () => {
        try {
            const response = await instance.loginPopup(loginRequest);
            setAccount(response.account);
            setAccessToken(response.accessToken);
            setIsAuthenticated(true);
            setLocalStorageItem(LOCAL_STORAGE_KEYS.ACCESS_TOKEN, response.accessToken);
            setLocalStorageItem(LOCAL_STORAGE_KEYS.ID_TOKEN, response.idToken);
            setLocalStorageItem(LOCAL_STORAGE_KEYS.USERNAME, response.account.username);
        } catch (error) {
            console.error(error);
        }
    };

    const logout = () => {
        instance.logoutPopup();
        setAccount(null);
        setAccessToken(null);
        setIsAuthenticated(false);
        navigate('/');
        removeLocalStorageItem(LOCAL_STORAGE_KEYS.ACCESS_TOKEN);
        removeLocalStorageItem(LOCAL_STORAGE_KEYS.ID_TOKEN);
        removeLocalStorageItem(LOCAL_STORAGE_KEYS.USERNAME);
    };

    return (
        <AuthContext.Provider value={{ account, accessToken, isAuthenticated, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = (): AuthContextType => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};