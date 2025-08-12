import { useState, useEffect } from 'react';
import { Grid, Paper, Stack, TextField } from '@mui/material';
import { deepValueChange } from '../../utils/deepValueChange';
import createId from '../../utils/createId';
import { useNavigate, useParams } from 'react-router-dom';
import { useNotification } from '../../contexts/notificationContext';
import DefaultContentContainer from '../../components/contentBlocks/DefaultContentBlock';
import { CreateActions } from '../../components/contentBlocks/PageActionsBlock';
import { addCatalogGroup } from '../../services/catalogs.service';
import { AddCatalogGroupCommand, CatalogGroupId } from '../../types/domain';
import { handleAsyncTask } from '../../utils/handleAsyncTask';

const CatalogGroupCreate = () => {
    const params = useParams();
    const [data, setData] = useState<AddCatalogGroupCommand>({
        catalogGroupId: createId<CatalogGroupId>(),
        catalogId: { value: params.catalogId! },
        name: '',
        description: '',        
    });
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const navigate = useNavigate();
    const { toast } = useNotification();

    useEffect(() => {
        setLoading(false);
    }, []);

    const handleChange = (e: any) => {
        const { name, value } = e.target;
        setData(deepValueChange(data, name, value));
    };

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();

        handleAsyncTask({
            task: () => addCatalogGroup(params.catalogId!, data),
            onLoading: setLoading,
            onSuccess: () => {
                toast("Erfolgreich erstellt", "success");
                navigate(`/catalogs/${data?.catalogId!.value}`);
            },
            onError: setError
        });
    };

    return (
        <form id="form" onSubmit={handleSubmit}>
            <Stack spacing={2}>
                <DefaultContentContainer subtitle={data?.name} loading={loading} error={error} >
                    <Paper sx={{ p: 2 }}>
                        <Grid container spacing={2}>
                            <Grid size={{ xs: 12 }}>
                                <TextField fullWidth label="Name" name="name" value={data?.name} onChange={handleChange} required />
                            </Grid>
                            <Grid size={{ xs: 12 }}>
                                <TextField fullWidth label="Beschreibung" name="description" value={data?.description} onChange={handleChange} />
                            </Grid>
                        </Grid>
                    </Paper>
                </DefaultContentContainer >
                <CreateActions formId="form" disabled={loading} />
            </Stack>
        </form>
    );
};

export default CatalogGroupCreate;
