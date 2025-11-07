import { useState, useEffect } from 'react';
import { CatalogGroupDTO } from '../../types/domain';
import { Paper, Stack } from '@mui/material';
import DetailPageContainer from '../../components/ContentBlocks/DefaultContentBlock';
import { useNavigate, useParams } from 'react-router-dom';
import { useNotification } from '../../contexts/notificationContext';
import { DeleteActions } from '../../components/ContentBlocks/PageActionsBlock';
import { getCatalogGroup, removeCatalogGroup } from '../../services/catalogs.service';
import { handleAsyncTask } from '../../utils/handleAsyncTask';

const CatalogGroupDelete = () => {
    const [data, setData] = useState<CatalogGroupDTO>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const navigate = useNavigate();
    const params = useParams();
    const { toast } = useNotification();

    useEffect(() => {
        handleAsyncTask({
            task: () => getCatalogGroup(params.catalogId!, params.catalogGroupId!),
            onLoading: setLoading,
            onSuccess: (response) => {
                setData(response);
            },
            onError: setError
        });
    }, []);

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        handleAsyncTask({
            task: () => removeCatalogGroup(params.catalogId!, params.catalogGroupId!),
            onLoading: setLoading,
            onSuccess: () => {
                toast("Gruppe erfolgreich gelöscht", "success");
                navigate(`/catalogs/${params.catalogId}`);
            },
            onError: setError
        });
    };

    return (
        <form id="form" onSubmit={handleSubmit}>
            <Stack spacing={2}>
                <DetailPageContainer subtitle={data?.name} loading={loading} error={error} >
                    <Paper sx={{ p: 2 }}>
                        Soll die Gruppe "{data?.name}" wirklich gelöscht werden?
                    </Paper>
                </DetailPageContainer >
                <DeleteActions formId="form" />
            </Stack>
        </form>
    );
};

export default CatalogGroupDelete;
