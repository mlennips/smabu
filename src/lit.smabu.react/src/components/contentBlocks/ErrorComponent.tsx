import { useState } from "react";
import { AppError } from "../../utils/errorConverter";
import { Alert, AlertTitle, Collapse, IconButton } from "@mui/material";
import { Close } from "@mui/icons-material";

const ErrorComponent = (error: AppError) => {
    const [isOpen, setIsOpen] = useState<boolean>(true);

    return <Collapse in={isOpen}>
        <Alert
            severity={error.severity}
            variant='standard'
            action={
                <IconButton
                    aria-label="close"
                    color="inherit"
                    size="small"
                    onClick={() => setIsOpen(false)}
                >
                    <Close fontSize="inherit" />
                </IconButton>
            }>
            <AlertTitle>{error.message}</AlertTitle>
            {error.details}
        </Alert></Collapse>
}

export default ErrorComponent;