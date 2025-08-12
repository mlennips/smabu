import { useState, useEffect } from 'react';
import { CatalogGroupDTO } from '../../types/domain';
import { Grid, Paper, Stack, TextField } from '@mui/material';
import DefaultContentContainer, { ToolbarItem } from '../../components/contentBlocks/DefaultContentBlock';
import { deepValueChange } from '../../utils/deepValueChange';
import { DetailsActions } from '../../components/contentBlocks/PageActionsBlock';
import { getCatalogGroup, updateCatalogGroup } from '../../services/catalogs.service';
import { useNotification } from '../../contexts/notificationContext';
import { useParams } from 'react-router-dom';
import { handleAsyncTask } from '../../utils/handleAsyncTask';

const CatalogGroupDetails = () => {
    const params = useParams();
    const [data, setData] = useState<CatalogGroupDTO>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const { toast } = useNotification();

    useEffect(() => {
        loadData();
    }, []);

    const loadData = () => handleAsyncTask({
        task: () => getCatalogGroup(params.catalogId!, params.catalogGroupId!),
        onLoading: setLoading,
        onSuccess: (response) => {
            setData(response);
        },
        onError: setError
    });
    

    const handleChange = (e: any) => {
        const { name, value } = e.target;
        setData(deepValueChange(data, name, value));
    };
    
    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        setLoading(true);
        updateCatalogGroup(params.catalogId!, params.catalogGroupId!, {
            catalogGroupId: data?.id!,
            catalogId: data?.catalogId!,
            name: data?.name!,
            description: data?.description!
        })
            .then(() => {
                setLoading(false);
                toast("Erfolgreich gespeichert", "success");
            })
            .catch(error => {
                setError(error);
                setLoading(false);
            });
    };
    


    const toolbarDetails: ToolbarItem[] = [];

    return (
        <Stack spacing={2}>
            <DefaultContentContainer subtitle={data?.displayName} loading={loading} error={error} toolbarItems={toolbarDetails} >
                <form id="form" onSubmit={handleSubmit} >
                    <Paper sx={{ p: 2 }}>
                        <Grid container spacing={2}>
                            <Grid size={{ xs: 12, md: 4 }}>
                                <TextField
                                    fullWidth
                                    id="name"
                                    name="name"
                                    label="Name"
                                    value={data?.name}
                                    onChange={handleChange}
                                />
                            </Grid>
                            <Grid size={{ xs: 12, md: 8 }}>
                                <TextField
                                    fullWidth
                                    id="description"
                                    name="description"
                                    label="Beschreibung"
                                    value={data?.description}
                                    onChange={handleChange}
                                />
                            </Grid>
                        </Grid>
                    </Paper>
                </form>
            </DefaultContentContainer >
            <DetailsActions formId="form" deleteUrl={`/catalogs/${params.catalogId}/groups/${params.catalogGroupId}/delete`} disabled={loading} />
        </Stack>
    );
};

export default CatalogGroupDetails;