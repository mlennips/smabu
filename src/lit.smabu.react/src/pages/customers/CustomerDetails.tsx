import { useState, useEffect } from 'react';
import { CustomerDTO } from '../../types/domain';
import { useParams } from 'react-router-dom';
import { Avatar, AvatarGroup, Grid2 as Grid, Paper, Stack, TextField, Typography } from '@mui/material';
import DefaultContentContainer, { ToolbarItem } from '../../components/contentBlocks/DefaultContentBlock';
import { deepValueChange } from '../../utils/deepValueChange';
import { useNotification } from '../../contexts/notificationContext';
import { getCustomer, updateCustomer } from '../../services/customer.service';
import { DetailsActions } from '../../components/contentBlocks/PageActionsBlock';
import { handleAsyncTask } from '../../utils/handleAsyncTask';
import { PaymentMethodSelectField } from '../../components/controls/SelectField';

const CustomerDetails = () => {
    const params = useParams();
    const { toast } = useNotification();
    const [data, setData] = useState<CustomerDTO>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(undefined);
    const toolbarItems: ToolbarItem[] = [];

    useEffect(() => {
        handleAsyncTask({
            task: () => getCustomer(params.customerId!),
            onLoading: setLoading,
            onSuccess: setData,
            onError: setError
        });
    }, [params.customerId]);

    const handleChange = (e: any) => {
        let { name, value } = e.target;
        if (name === "corporateDesign.color1" || name === "corporateDesign.color2") {
            value = { hex: value };
        }
        if (name === "corporateDesign.logo") {
            value = { fileUrl: value };
        }
        setData(deepValueChange(data, name, value));
    };

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        handleAsyncTask({
            task: () => updateCustomer(params.customerId!, {
                customerId: data?.id!,
                name: data?.name!,
                industryBranch: data?.industryBranch!,
                mainAddress: data?.mainAddress,
                communication: data?.communication,
                corporateDesign: data?.corporateDesign,
                vatId: data?.vatId,
                preferredPaymentMethod: data?.preferredPaymentMethod
            }),
            onLoading: setLoading,
            onSuccess: () => {
                toast("Kunde erfolgreich gespeichert", "success");
            },
            onError: setError
        });
    };

    return (
        <form id="form" onSubmit={handleSubmit}>
            <Stack spacing={2}>
                <Grid container spacing={2}>
                    <Grid size={{ xs: 12 }}>
                        <Paper sx={{ p: 2, mt: 2, mb: -10, background: 'linear-gradient(to bottom, #eee, #f5f5f5)', border: 0 }} variant='outlined'>
                            <Grid size={{ xs: 12 }} sx={{ mt: 1, mb: 8 }} container component="header">
                                <Grid size="auto">
                                    <AvatarGroup sx={{ opacity: 0.8 }}>
                                        <Avatar sx={{ width: 52, height: 52, bgcolor: data?.corporateDesign?.color1?.hex }}>
                                            &nbsp;
                                        </Avatar>
                                        <Avatar sx={{ width: 52, height: 52, bgcolor: data?.corporateDesign?.color2?.hex }}>
                                            &nbsp;
                                        </Avatar>
                                    </AvatarGroup>
                                </Grid>
                                <Grid size="grow" sx={{ ml: 1 }}>
                                    <Typography variant="h5" fontWeight={600}>{data?.corporateDesign?.brand}</Typography>
                                    <Typography variant="subtitle2">{data?.corporateDesign?.slogan}</Typography>
                                </Grid>
                                <Grid size="auto" textAlign='end' sx={{ display: { xs: 'none', sm: 'initial' } }}>
                                    {data?.corporateDesign?.logo?.fileUrl && <img src={data?.corporateDesign?.logo?.fileUrl ?? undefined} alt="Logo"
                                        style={{ maxHeight: '42px', minWidth: '100px' }} />}
                                </Grid>
                            </Grid>
                        </Paper>
                    </Grid>
                    <Grid size={{ xs: 12 }}>
                        <DefaultContentContainer subtitle={data?.name} loading={loading} error={error} toolbarItems={toolbarItems} >
                            <Paper sx={{ p: 2 }}>
                                <Grid container spacing={2}>
                                    <Grid size={{ xs: 6, sm: 3, md: 2 }}><TextField fullWidth label="#" name="number"
                                        value={data?.number?.displayName} disabled /></Grid>
                                    <Grid size={{ xs: 6, sm: 3, md: 2 }}><TextField fullWidth label="Branche" name="industryBranch"
                                        value={data?.industryBranch} onChange={handleChange} required /></Grid>
                                    <Grid size={{ xs: 12, sm: 6, md: 8 }}><TextField fullWidth label="Name" name="name"
                                        value={data?.name} onChange={handleChange} required /></Grid>
                                    <Grid size={{ xs: 2, sm: 2, md: 2 }}><TextField fullWidth label="Währung" name="currency"
                                        value={data?.currency?.name} disabled /></Grid>
                                    <Grid size={{ xs: 10, sm: 4, md: 4 }}><TextField fullWidth label="USt-Nr" name="vatId"
                                        value={data?.vatId} onChange={handleChange} /></Grid>
                                    <Grid size={{ xs: 12, sm: 6, md: 6 }}><PaymentMethodSelectField label='Bevorzugte Zahlungsweise' name="preferredPaymentMethod" value={data?.preferredPaymentMethod?.value}
                                        onChange={handleChange} required /></Grid>
                                </Grid>
                            </Paper>
                        </DefaultContentContainer >
                    </Grid>
                    <Grid size={{ xs: 12, md: 8 }}>
                        <DefaultContentContainer title="Adresse" loading={loading} error={error} >
                            <Paper sx={{ p: 2 }}><div>
                                <Grid container spacing={2}>
                                    <Grid size={{ xs: 12 }}>
                                        <TextField fullWidth label="Name 1" name="mainAddress.name1" value={data?.mainAddress?.name1} onChange={handleChange} />
                                    </Grid>
                                    <Grid size={{ xs: 12 }}>
                                        <TextField fullWidth label="Name 2" name="mainAddress.name2" value={data?.mainAddress?.name2} onChange={handleChange} />
                                    </Grid>
                                    <Grid size={{ xs: 9, sm: 8 }}>
                                        <TextField fullWidth label="Straße" name="mainAddress.street" value={data?.mainAddress?.street} onChange={handleChange} />
                                    </Grid>
                                    <Grid size={{ xs: 3, sm: 4 }}>
                                        <TextField fullWidth label="Nr." name="mainAddress.houseNumber" value={data?.mainAddress?.houseNumber} onChange={handleChange} />
                                    </Grid>
                                    <Grid size={{ xs: 12, sm: 3 }}>
                                        <TextField fullWidth label="PLZ" name="mainAddress.postalCode" value={data?.mainAddress?.postalCode} onChange={handleChange} />
                                    </Grid>
                                    <Grid size={{ xs: 12, sm: 6 }}>
                                        <TextField fullWidth label="Ort" name="mainAddress.city" value={data?.mainAddress?.city} onChange={handleChange} />
                                    </Grid>
                                    <Grid size={{ xs: 12, sm: 3 }}>
                                        <TextField fullWidth label="Land" name="mainAddress.country" value={data?.mainAddress?.country} onChange={handleChange} />
                                    </Grid>
                                </Grid></div>
                            </Paper>
                        </DefaultContentContainer >
                    </Grid>
                    <Grid size={{ xs: 12, md: 4 }}>
                        <DefaultContentContainer title="Kommunikation" loading={loading} error={error} >
                            <Paper sx={{ p: 2 }}>
                                <Grid container spacing={2}>
                                    <Grid size={{ xs: 12 }}>
                                        <TextField fullWidth label="Email" name="communication.email" value={data?.communication?.email} onChange={handleChange} />
                                    </Grid>
                                    <Grid size={{ xs: 12 }}>
                                        <TextField fullWidth label="Mobil" name="communication.mobil" value={data?.communication?.mobil} onChange={handleChange} />
                                    </Grid>
                                    <Grid size={{ xs: 12 }}>
                                        <TextField fullWidth label="Telefon" name="communication.phone" value={data?.communication?.phone} onChange={handleChange} />
                                    </Grid>
                                    <Grid size={{ xs: 12 }}>
                                        <TextField fullWidth label="Website" name="communication.website" value={data?.communication?.website} onChange={handleChange} />
                                    </Grid>
                                </Grid>
                            </Paper>
                        </DefaultContentContainer >
                    </Grid>
                    <Grid size={{ xs: 12 }}>
                        <DefaultContentContainer title="Corporate Design" loading={loading} error={error} >
                            <Paper sx={{ p: 2 }}>
                                <Grid container spacing={2}>
                                    <Grid size={{ xs: 12, sm: 6 }}>
                                        <TextField fullWidth label="Marke" name="corporateDesign.brand"
                                            value={data?.corporateDesign?.brand} onChange={handleChange} />
                                    </Grid>
                                    <Grid size={{ xs: 12, sm: 6 }}>
                                        <TextField fullWidth label="Kurzname" name="corporateDesign.shortName"
                                            value={data?.corporateDesign?.shortName} onChange={handleChange} />
                                    </Grid>
                                    <Grid size={{ xs: 12, sm: 12 }}>
                                        <TextField fullWidth label="Slogan" name="corporateDesign.slogan"
                                            value={data?.corporateDesign?.slogan} onChange={handleChange} />
                                    </Grid>
                                    <Grid size={{ xs: 12, sm: 6 }}>
                                        <TextField type='color' fullWidth label="Primärfarbe" name="corporateDesign.color1"
                                            value={data?.corporateDesign?.color1?.hex} onChange={handleChange} />
                                    </Grid>
                                    <Grid size={{ xs: 12, sm: 6 }}>
                                        <TextField type='color' fullWidth label="Sekundärfarbe" name="corporateDesign.color2"
                                            value={data?.corporateDesign?.color2?.hex} onChange={handleChange} />
                                    </Grid>
                                    <Grid size={{ xs: 12, sm: 12 }}>
                                        <TextField fullWidth label="Logo URL" name="corporateDesign.logo" value={data?.corporateDesign?.logo?.fileUrl} onChange={handleChange} />
                                    </Grid>
                                </Grid>
                            </Paper>
                        </DefaultContentContainer >
                    </Grid>
                </Grid>
                <DetailsActions formId="form" deleteUrl={`/customers/${data?.id?.value}/delete`} disabled={loading} />
            </Stack>
        </form>
    );
};

export default CustomerDetails;


