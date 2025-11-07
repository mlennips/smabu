import { useState, useEffect } from 'react';
import { CatalogItemDTO } from '../../types/domain';
import { Paper, Stack } from '@mui/material';
import DetailPageContainer from '../../components/ContentBlocks/DefaultContentBlock';
import { useNavigate, useParams } from 'react-router-dom';
import { useNotification } from '../../contexts/notificationContext';
import { DeleteActions } from '../../components/ContentBlocks/PageActionsBlock';
import { getCatalogItem, removeCatalogItem } from '../../services/catalogs.service';
import { handleAsyncTask } from '../../utils/handleAsyncTask';

const CatalogItemDelete = () => {
    const [data, setData] = useState<CatalogItemDTO>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const navigate = useNavigate();
    const params = useParams();
    const { toast } = useNotification();

    useEffect(() => {
        handleAsyncTask({
            task: () => getCatalogItem(params.catalogId!, params.catalogItemId!),
            onLoading: setLoading,
            onSuccess: setData,
            onError: setError
        });
    }, []);

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        handleAsyncTask({
            task: () => removeCatalogItem(params.catalogId!, params.catalogItemId!),
            onLoading: setLoading,
            onSuccess: () => {
                toast("Artikel erfolgreich gelöscht", "success");
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
                        Soll der Artikel "{data?.name}" wirklich gelöscht werden?
                    </Paper>
                </DetailPageContainer >
                <DeleteActions formId="form" />
            </Stack>
        </form>
    );
};

export default CatalogItemDelete;
