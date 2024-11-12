import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Grid2 as Grid, Paper, Stack, TextField } from '@mui/material';
import DefaultContentContainer from '../../components/contentBlocks/DefaultContentBlock';
import { deepValueChange } from '../../utils/deepValueChange';
import { useNotification } from '../../contexts/notificationContext';
import createId from '../../utils/createId';
import { getOffer, addOfferItem } from '../../services/offer.service';
import { OfferDTO, AddOfferItemCommand } from '../../types/domain';
import { CreateActions } from '../../components/contentBlocks/PageActionsBlock';
import { UnitSelectField } from '../../components/controls/SelectField';
import React from 'react';
import SelectCatalogItemComponent from '../catalogs/SelectCatalogItemComponent';
import { handleAsyncTask } from '../../utils/handleAsyncTask';

const OfferItemCreate = () => {
    const params = useParams();
    const navigate = useNavigate();
    const { toast } = useNotification();
    const [offer, setOffer] = useState<OfferDTO>();
    const [data, setData] = useState<AddOfferItemCommand>({
        offerItemId: createId(),
        offerId: { value: params.offerId },
        quantity: { value: 0, unit: undefined },
        unitPrice: 0,
        details: "",
    });
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);

    useEffect(() => {
        handleAsyncTask({
            task: () => getOffer(params.offerId!, true),
            onLoading: setLoading,
            onSuccess: setOffer,
            onError: setError
        });
    }, []);

    const handleChange = (e: any) => {
        let { name, value } = e.target;
        setData(deepValueChange(data, name, value));
    };

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();

        handleAsyncTask({
            task: () => addOfferItem(params.offerId!, data),
            onLoading: setLoading,
            onSuccess: () => {
                toast("Angebotsposition erfolgreich erstellt", "success");
                navigate(`/offers/${params.offerId}`);
            },
            onError: setError
        });
    };

    return (
        <form id="form" onSubmit={handleSubmit}>
            <Stack spacing={2}>
                <DefaultContentContainer subtitle={offer?.displayName} loading={loading} error={error} >
                    <Paper sx={{ p: 2 }}>
                        <Grid container spacing={2}>
                            <Grid size={{ xs: 12, sm: 6, md: 6 }}><TextField fullWidth label="Angebot" name="offer" value={offer?.displayName} disabled /></Grid>
                            <Grid size={{ xs: 12, sm: 6, md: 6 }}><TextField fullWidth label="Kunde" name="customer" value={offer?.customer?.name} disabled /></Grid>
                        </Grid>
                    </Paper>
                </DefaultContentContainer >

                <DefaultContentContainer title="Details" loading={loading}>
                    <Paper sx={{ p: 2 }}>
                        <Stack>
                            <SelectCatalogItemComponent 
                                customerId={offer?.customer?.id!}
                                getCatalogItemId={() => data.catalogItemId}
                                setCatalogItemId={(value) => setData(deepValueChange(data, "catalogItemId", value))}
                                setDetails={(value) => setData(deepValueChange(data, 'details', value))}
                                setPrice={(value) => setData(deepValueChange(data, 'unitPrice', value))} 
                                setUnit={(value) => setData(deepValueChange(data, 'quantity.unit', value))}  />
                            <TextField multiline variant='standard' minRows={5} maxRows={10} fullWidth
                                name="details" value={data?.details} onChange={handleChange} />
                        </Stack>
                    </Paper>
                </DefaultContentContainer>
                <DefaultContentContainer title="Menge und Preis" loading={loading} error={error} >
                    <Paper sx={{ p: 2 }}>
                        <Grid container spacing={2}>
                            <Grid size={{ xs: 6, sm: 6, md: 3 }}><TextField type='number' fullWidth label="Anzahl" name="quantity.value" value={data?.quantity?.value} onChange={handleChange} required /></Grid>
                            <Grid size={{ xs: 6, sm: 6, md: 3 }}>
                                <UnitSelectField label="Einheit" name="quantity.unit" value={data?.quantity?.unit?.value} required onChange={handleChange} />
                            </Grid>
                            <Grid size={{ xs: 6, sm: 6, md: 3 }}><TextField type='number' fullWidth label="Einzelpreis" name="unitPrice" value={data?.unitPrice} onChange={handleChange} required /></Grid>
                            <Grid size={{ xs: 6, sm: 6, md: 3 }}><TextField type='number' fullWidth label="Gesamt" name="totalPrice" value={(data.unitPrice * (data.quantity?.value ?? 0))} disabled /></Grid>
                        </Grid>
                    </Paper>
                </DefaultContentContainer >

                <CreateActions formId="form" disabled={loading} />
            </Stack>
        </form>
    );
};

export default OfferItemCreate;