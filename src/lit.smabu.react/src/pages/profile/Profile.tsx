import { useEffect, useState } from "react";

// Msal imports
import { MsalAuthenticationTemplate, useMsal } from "@azure/msal-react";
import { InteractionStatus, InteractionType, InteractionRequiredAuthError, AccountInfo } from "@azure/msal-browser";
import { loginRequest } from "../../configs/authConfig.ts";

// Sample app imports
import { ProfileData, GraphData } from "./ProfileData.tsx";
import { Loading } from "./Loading.tsx";
import { ErrorComponent } from "./ErrorComponent.tsx";
import { CallMsGraph } from "../../utils/msGraphApiCall.ts";

// Material-ui imports
import Paper from "@mui/material/Paper";

const ProfileContent = () => {
    const { instance, inProgress } = useMsal();
    const [graphData, setGraphData] = useState<null|GraphData>(null);

    useEffect(() => {
        if (!graphData && inProgress === InteractionStatus.None) {
            CallMsGraph().then(response => setGraphData(response)).catch((e) => {
                if (e instanceof InteractionRequiredAuthError) {
                    instance.acquireTokenRedirect({
                        ...loginRequest,
                        account: instance.getActiveAccount() as AccountInfo
                    });
                }
            });
        }
    }, [inProgress, graphData, instance]);
  
    return (
        <Paper>
            { graphData ? <ProfileData graphData={graphData} /> : null }
        </Paper>
    );
};

export function Profile() {
    const authRequest = {
        ...loginRequest
    };

    return (
        <MsalAuthenticationTemplate 
            interactionType={InteractionType.Redirect} 
            authenticationRequest={authRequest} 
            errorComponent={ErrorComponent} 
            loadingComponent={Loading}
        >
            <ProfileContent />
        </MsalAuthenticationTemplate>
      )
};