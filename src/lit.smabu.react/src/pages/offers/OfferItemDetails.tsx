import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { Grid2 as Grid, Paper, Stack, TextField } from '@mui/material';
import DefaultContentContainer, { ToolbarItem } from '../../components/contentBlocks/DefaultContentBlock';
import { deepValueChange } from '../../utils/deepValueChange';
import { useNotification } from '../../contexts/notificationContext';
import { getOffer, updateOfferItem } from '../../services/offer.service';
import { OfferDTO, OfferItemDTO } from '../../types/domain';
import { DetailsActions } from '../../components/contentBlocks/PageActionsBlock';
import { UnitSelectField } from '../../components/controls/SelectField';
import SelectCatalogItemComponent from '../catalogs/SelectCatalogItemComponent';
import { handleAsyncTask } from '../../utils/handleAsyncTask';

const OfferItemDetails = () => {
    const params = useParams();
    const { toast } = useNotification();
    const [offer, setOffer] = useState<OfferDTO>();
    const [data, setData] = useState<OfferItemDTO>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const toolbarItems: ToolbarItem[] = [];

    useEffect(() => {
        loadData();
    }, [params.offerItemId]);

    const loadData = () => handleAsyncTask({
        task: () => getOffer(params.offerId!, true),
        onLoading: setLoading,
        onSuccess: (response) => {
            setOffer(response);
            setData(response.items?.find((item: OfferItemDTO) => item.id!.value === params.offerItemId));
        },
        onError: setError
    });

    const handleChange = (e: any) => {
        let { name, value } = e.target;
        setData(deepValueChange(data, name, value));
    };

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        
        handleAsyncTask({
            task: () => updateOfferItem(params.offerId!, params.offerItemId!, {
                offerItemId: data?.id!,
                offerId: data?.offerId!,
                quantity: data?.quantity!,
                unitPrice: data?.unitPrice!,
                details: data?.details!,
                catalogItemId: data?.catalogItemId!
            }),
            onLoading: setLoading,
            onSuccess: () => {
                toast("Angebotsposition erfolgreich gespeichert", "success");
                loadData();
            },
            onError: setError
        });
    };

    return (
        <form id="form" onSubmit={handleSubmit}>
            <Stack spacing={2}>
                <DefaultContentContainer title={offer?.displayName} subtitle={"#" + data?.displayName} loading={loading} error={error} toolbarItems={toolbarItems} >
                    <Paper sx={{ p: 2 }}>
                        <Grid container spacing={2}>
                            <Grid size={{ xs: 12, sm: 2, md: 2 }}><TextField fullWidth label="Position" name="position" value={data?.position} disabled /></Grid>
                            <Grid size={{ xs: 12, sm: 5, md: 4 }}><TextField fullWidth label="Angebot" name="offer" value={offer?.displayName} disabled /></Grid>
                            <Grid size={{ xs: 12, sm: 5, md: 6 }}><TextField fullWidth label="Kunde" name="customer" value={offer?.customer?.name} disabled /></Grid>
                        </Grid>
                    </Paper>
                </DefaultContentContainer >

                <DefaultContentContainer title="Details" loading={loading}>
                    <SelectCatalogItemComponent
                        customerId={offer?.customer?.id!}
                        getCatalogItemId={() => data?.catalogItemId}
                        setCatalogItemId={(value) => setData(deepValueChange(data, "catalogItemId", value))}
                        setDetails={(value) => setData(deepValueChange(data, 'details', value))}
                        setPrice={(value) => setData(deepValueChange(data, 'unitPrice', value))}
                        setUnit={(value) => setData(deepValueChange(data, 'quantity.unit', value))} />
                    <TextField multiline variant='standard' minRows={5} maxRows={10} fullWidth
                        name="details" value={data?.details} onChange={handleChange} />
                </DefaultContentContainer>

                <DefaultContentContainer title="Menge und Preis" loading={loading} error={error} toolbarItems={toolbarItems} >
                    <Paper sx={{ p: 2 }}>
                        <Grid container spacing={2}>
                            <Grid size={{ xs: 6, sm: 6, md: 3 }}><TextField type='number' fullWidth label="Anzahl" name="quantity.value" value={data?.quantity?.value} onChange={handleChange} required /></Grid>
                            <Grid size={{ xs: 6, sm: 6, md: 3 }}>
                                <UnitSelectField label="Einheit" name="quantity.unit" value={data?.quantity?.unit?.value} required onChange={handleChange} />
                            </Grid>
                            <Grid size={{ xs: 6, sm: 6, md: 3 }}><TextField type='number' fullWidth label="Einzelpreis" name="unitPrice" value={data?.unitPrice} onChange={handleChange} required /></Grid>
                            <Grid size={{ xs: 6, sm: 6, md: 3 }}><TextField type='number' fullWidth label="Gesamt" name="totalPrice" value={data?.totalPrice} disabled /></Grid>
                        </Grid>
                    </Paper>
                </DefaultContentContainer >

                <DetailsActions formId="form" deleteUrl={`/offers/${params.offerId}/items/${data?.id?.value}/delete`} disabled={loading} />
            </Stack>
        </form>
    );
};

export default OfferItemDetails;


