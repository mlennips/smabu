import { useState, useEffect } from 'react';
import { AnnualFinancialStatementId, CreateAnnualFinancialStatementCommand } from '../../types/domain';
import { Grid2 as Grid, Paper, Stack, TextField } from '@mui/material';
import { deepValueChange } from '../../utils/deepValueChange';
import createId from '../../utils/createId';
import { useNavigate } from 'react-router-dom';
import { useNotification } from '../../contexts/notificationContext';
import DefaultContentContainer from '../../components/contentBlocks/DefaultContentBlock';
import { CreateActions } from '../../components/contentBlocks/PageActionsBlock';
import { handleAsyncTask } from '../../utils/handleAsyncTask';
import { createAnnualFinancialStatement } from '../../services/financials.service';

const AnnualFinancialStatementCreate = () => {
    const [data, setData] = useState<CreateAnnualFinancialStatementCommand>({
        annualFinancialStatementId: createId<AnnualFinancialStatementId>(),
        fiscalYear: new Date().getFullYear(),
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
            task: () => createAnnualFinancialStatement(data),
            onLoading: setLoading,
            onSuccess: () => {
                toast("Jahresabschluss erfolgreich erstellt", "success");
                navigate(`/financial/annualstatements/${data.annualFinancialStatementId?.value}`);
            },
            onError: setError

        });
    };

    return (
        <form id="form" onSubmit={handleSubmit}>
            <Stack spacing={2}>
                <DefaultContentContainer subtitle={`${data?.fiscalYear}`} loading={loading} error={error} >
                    <Paper sx={{ p: 2 }}>
                        <Grid container spacing={1}>
                            <Grid size={{ xs: 12 }}>
                                <TextField
                                    fullWidth
                                    label="Geschäftsjahr"
                                    name="fiscalYear"
                                    type="number"
                                    value={data?.fiscalYear}
                                    onChange={handleChange}
                                    required
                                    slotProps={{
                                        htmlInput: {
                                            min: new Date().getFullYear() - 2,
                                            max: new Date().getFullYear()
                                        }
                                    }}
                                />
                            </Grid>
                        </Grid>
                    </Paper>
                </DefaultContentContainer >
                <CreateActions formId="form" disabled={loading} />
            </Stack>
        </form>
    );
};

export default AnnualFinancialStatementCreate;
