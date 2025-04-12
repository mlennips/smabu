import { ArrowBack, ArrowForward as ArrowForwardIcon, Cancel as CancelIcon, Delete as DeleteIcon, Refresh as RefreshIcon, Save as SaveIcon } from '@mui/icons-material';
import { Button, ButtonGroup, Divider, Grid, IconButton } from '@mui/material';
import React from 'react';

interface DetailsActionsProps {
    formId: string | undefined;
    deleteUrl?: string;
    disabled?: boolean;
}

export const DetailsActions: React.FC<DetailsActionsProps> = ({ formId, deleteUrl, disabled }) => {
    return (
        <Grid container sx={{ my:2 }}>
            <Grid size="auto">
                <ButtonGroup>
                    <IconButton color="default" onClick={() => window.history.back()}
                        title='Zurück'>
                        <ArrowBack />
                    </IconButton>
                </ButtonGroup>
            </Grid>
            <Grid size="grow">

            </Grid>
            <Grid size="auto">
                <ButtonGroup variant="text" >                    
                    {deleteUrl && <IconButton color="error" href={deleteUrl}
                        title='Löschen'>
                        <DeleteIcon />
                    </IconButton>}
                </ButtonGroup>
            </Grid>
            <Grid size="auto">   
                <Divider orientation="vertical" sx={{ mr: 3, ml: 1 }} />
            </Grid>
            <Grid size="auto">
                <ButtonGroup variant="contained" >
                    <IconButton color="info"
                        href={location.href}
                        title='Neu laden'                        
                    >
                        <RefreshIcon />
                    </IconButton>
                    {formId && <Button type="submit" form={formId} color="info"
                        title='Speichern'
                        disabled={disabled}
                        endIcon={<SaveIcon />}
                    >
                        Speichern
                    </Button>}
                </ButtonGroup>
            </Grid>
        </Grid>
    );
};

export const CreateActions: React.FC<DetailsActionsProps> = ({ formId, disabled }) => {
    return (
        <Grid container sx={{ my:2 }}>
            <Grid size="auto">
                <ButtonGroup variant='contained'>
                    <Button color="warning" 
                        onClick={() => window.history.back()}
                        title='Abbrechen'
                        startIcon={<CancelIcon />}
                    >
                        Abbrechen
                    </Button>
                </ButtonGroup>
            </Grid>
            <Grid size="grow">

            </Grid>
            <Grid size="auto">
                <ButtonGroup variant="contained" >
                    {formId && <Button type="submit" form={formId} color="success"
                        title='Weiter'
                        disabled={disabled}
                        endIcon={<ArrowForwardIcon />}
                    >
                        Weiter
                    </Button>}
                </ButtonGroup>
            </Grid>
        </Grid>
    );
};

export const DeleteActions: React.FC<DetailsActionsProps> = ({ formId, disabled }) => {
    return (
        <Grid container sx={{ my:2 }}>
            <Grid size="auto">
                <ButtonGroup variant='contained'>
                    <Button color="warning" 
                        onClick={() => window.history.back()}
                        title='Abbrechen'
                        startIcon={<CancelIcon />}
                    >
                        Abbrechen
                    </Button>
                </ButtonGroup>
            </Grid>
            <Grid size="grow">

            </Grid>
            <Grid size="auto">
                <ButtonGroup variant="contained" >
                    {formId && <Button type="submit" form={formId} color="error"
                        title='Löschen'
                        disabled={disabled}
                        endIcon={<DeleteIcon />}
                    >
                        Löschen
                    </Button>}
                </ButtonGroup>
            </Grid>
        </Grid>
    );
};
