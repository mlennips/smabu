import { useState, useEffect } from 'react';
import { Paper, Stack } from '@mui/material';
import DetailPageContainer from '../../components/ContentBlocks/DefaultContentBlock';
import { useNavigate, useParams } from 'react-router-dom';
import { useNotification } from '../../contexts/notificationContext';
import { getOffer, deleteOffer } from '../../services/offer.service';
import { OfferDTO } from '../../types/domain';
import { DeleteActions } from '../../components/ContentBlocks/PageActionsBlock';
import { handleAsyncTask } from '../../utils/handleAsyncTask';

const OfferDelete = () => {
    const [data, setData] = useState<OfferDTO>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const navigate = useNavigate();
    const params = useParams();
    const { toast } = useNotification();

    useEffect(() => {
        handleAsyncTask({
            task: () => getOffer(params.offerId!),
            onLoading: setLoading,
            onSuccess: setData,
            onError: setError
        });
    }, []);

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        handleAsyncTask({
            task: () => deleteOffer(params.offerId!),
            onLoading: setLoading,
            onSuccess: () => {
                toast("Angebot erfolgreich gelöscht", "success");
                navigate('/offers');
            },
            onError: setError
        });
    };

    return (
        <form id="form" onSubmit={handleSubmit}>
            <Stack spacing={2}>
                <DetailPageContainer subtitle={data?.number?.value?.toString()} loading={loading} error={error} >
                    <Paper sx={{ p: 2 }}>
                        Soll das Angebot "{data?.number?.value}" wirklich gelöscht werden?
                    </Paper>
                </DetailPageContainer >
                <DeleteActions formId="form" />
            </Stack>
        </form>
    );
};

export default OfferDelete;
