import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { Grid2 as Grid, Paper, Stack, TextField } from '@mui/material';
import DefaultContentContainer, { ToolbarItem } from '../../components/contentBlocks/DefaultContentBlock';
import { Print } from '@mui/icons-material';
import { useNotification } from '../../contexts/notificationContext';
import { getOffer, getOfferReport, updateOffer } from '../../services/offer.service';
import { OfferDTO } from '../../types/domain';
import OfferItemsComponent from './OfferItemsComponent';
import { deepValueChange } from '../../utils/deepValueChange';
import { openPdf } from '../../utils/openPdf';
import { DetailsActions } from '../../components/contentBlocks/PageActionsBlock';
import { formatForTextField } from '../../utils/formatDate';
import { handleAsyncTask } from '../../utils/handleAsyncTask';

const OfferDetails = () => {
    const params = useParams();
    const { toast } = useNotification();
    const [data, setData] = useState<OfferDTO>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const [errorItems, setErrorItems] = useState(null);
    const [toolbarItems, setToolbarItems] = useState<ToolbarItem[]>([]);
    const toolbarDetails: ToolbarItem[] = [
        {
            text: "PDF",
            action: () => pdf(),
            icon: <Print />,
        }
    ];

    useEffect(() => {
        loadData();
    }, [params.offerId]);

    const loadData = () =>
        handleAsyncTask({
            task: () => getOffer(params.offerId!, false),
            onLoading: setLoading,
            onSuccess: setData,
            onError: setError
        });

    const pdf = () => {
        handleAsyncTask({
            task: () => getOfferReport(params.offerId!),
            onLoading: setLoading,
            onSuccess: (report) => {
                openPdf(report.data, `Angebot_${data?.number?.value}_${data?.customer?.corporateDesign?.shortName}.pdf`);
            },
            onError: setError
        });
    };

    const handleChange = (e: any) => {
        const { name, value } = e.target;
        setData(deepValueChange(data, name, value));
    };

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        handleAsyncTask({
            task: () => updateOffer(params.offerId!, {
                offerId: data?.id!,
                taxRate: data?.taxRate!,
                expiresOn: data?.expiresOn!,
            }),
            onLoading: setLoading,
            onSuccess: () => {
                toast("Angebot erfolgreich gespeichert", "success");
            },
            onError: setError
        });
    };

    return (
    <Stack spacing={2}>
            <form id="form" onSubmit={handleSubmit} >
                <DefaultContentContainer subtitle={data?.displayName} loading={loading} error={error} toolbarItems={toolbarDetails} >
                    <Paper sx={{ p: 2 }}>
                        <Grid container spacing={2}>
                            <Grid size={{ xs: 12, sm: 4, md: 4 }}><TextField fullWidth label="#" name="number" value={data?.displayName} disabled /></Grid>
                            <Grid size={{ xs: 12, sm: 8, md: 8 }}><TextField fullWidth label="Kunde" name="customer.name" value={data?.customer?.name} disabled /></Grid>
                            <Grid size={{ xs: 12, sm: 6, md: 6 }}><TextField type="datetime-local" fullWidth label="Erstellt" name="createdOn" value={formatForTextField(data?.createdAt)} disabled /></Grid>
                            <Grid size={{ xs: 12, sm: 6, md: 6 }}><TextField type="date" fullWidth label="Läuft ab" name="expiresOn" value={data?.expiresOn?.toString()} onChange={handleChange} /></Grid>
                            <Grid size={{ xs: 12, sm: 2, md: 2 }}><TextField fullWidth label="Steuer" name="tax" value={data?.taxRate?.rate} required disabled /></Grid>
                            <Grid size={{ xs: 12, sm: 10, md: 10 }}><TextField fullWidth label="Steuerdetails" name="taxDetails" value={data?.taxRate?.details} disabled /></Grid>
                        </Grid>
                    </Paper>
                </DefaultContentContainer >
            </form>

            <DetailsActions formId="form" deleteUrl={`/offers/${data?.id?.value}/delete`} disabled={loading} />

            <DefaultContentContainer title="Positionen" loading={loading} error={errorItems} toolbarItems={toolbarItems} >
                <OfferItemsComponent offerId={params.offerId} setError={(error) => setErrorItems(error)} setToolbar={setToolbarItems} />
            </DefaultContentContainer >
        </Stack>
    );
};

export default OfferDetails;


