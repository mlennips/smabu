import { Configuration, PopupRequest } from "@azure/msal-browser";


// const domain = process.env.REACT_APP_AUTH0_DOMAIN;
// const clientId = process.env.REACT_APP_AUTH0_CLIENT_ID;
// const redirectUri = process.env.REACT_APP_AUTH0_CALLBACK_URL;
// const audience = process.env.REACT_APP_AUTH0_AUDIENCE;

// Config object to be passed to Msal on creation
export const msalConfig: Configuration = {
    auth: {
        clientId: "f5524605-80eb-471a-ae81-8b0c7ea83ad4",
        authority: "https://login.microsoftonline.com/common",
        redirectUri: "/",
        postLogoutRedirectUri: "/"
    },
    system: {
                
    }
};

// Add here scopes for id token to be used at MS Identity Platform endpoints.
export const loginRequest: PopupRequest = {
    scopes: ["User.Read"]
};

// Add here the endpoints for MS Graph API services you would like to use.
export const graphConfig = {
    graphMeEndpoint: "https://graph.microsoft.com/v1.0/me"
};