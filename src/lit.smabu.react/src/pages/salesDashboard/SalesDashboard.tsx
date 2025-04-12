import {
  Avatar, Box, Card, CardHeader, Container, Grid, Paper,
  Stack,
  Typography,
} from "@mui/material";
import { getSalesDashboard } from "../../services/dashboard.service";
import { useEffect, useState } from "react";
import { GetSalesDashboardReadModel } from "../../types/domain";
import { LineChart } from '@mui/x-charts/LineChart';
import { BarChart, PieChart } from "@mui/x-charts";
import { blueGrey, orange } from "@mui/material/colors";
import { Group, TrendingUp } from "@mui/icons-material";
import { cheerfulFiestaPalette } from '@mui/x-charts/colorPalettes';
import { handleAsyncTask } from "../../utils/handleAsyncTask";

const SalesDashboard = () => {
  const [data, setData] = useState<GetSalesDashboardReadModel>();

  useEffect(() => {
    handleAsyncTask({
      task: getSalesDashboard,
      onSuccess: setData,
    });
  }, []);

  return (
    <Container>
      {data && (
        <>
          {renderHeaderBlock(data)}
          {renderCustomerBlock(data)}
          {/* {mostValuableCustomers(data)} */}
          {render3YearsBlock()}
          {renderAverageBlock()}
        </>
      )}
    </Container>
  );
}

function renderHeaderBlock(data: GetSalesDashboardReadModel) {
  const salesYearSeries = data.salesByYear?.series?.find(serie => serie.key === 'year');
  const salesTotalSeries = data.salesByYear?.series?.find(serie => serie.key === 'total');

  return <Grid container spacing={0} sx={{ mt: 4 }}>
    <Grid size={{ xs: 6, sm: 6, md: 6, lg: 2 }}>
      <Card variant="outlined" sx={{ backgroundColor: 'transparent', border: 0 }}>
        <CardHeader
          avatar={<Avatar sx={{ bgcolor: orange[500] }}><Group /></Avatar>}
          title={data.customerCount}
          subheader='Kunden' />
      </Card>
      <Card variant="outlined" sx={{ backgroundColor: 'transparent', border: 0 }}>
        <CardHeader
          avatar={<Avatar sx={{ bgcolor: orange[500] }}><Group /></Avatar>}
          title='Coming soon'
          subheader='Aufträge' />
      </Card>
      <Card variant="outlined" sx={{ backgroundColor: 'transparent', border: 0 }}>
        <CardHeader
          avatar={<Avatar sx={{ bgcolor: orange[500] }}><Group /></Avatar>}
          title={data.invoiceCount}
          subheader='Rechnungen' />
      </Card>
    </Grid>

    <Grid size={{ xs: 6, sm: 6, md: 6, lg: 2 }} offset={{ xs: 0, sm: 0, md: 0, lg: 8 }}>
      <Stack direction="column" spacing={0} alignItems="flex-end">
        <Card variant="outlined" sx={{ backgroundColor: 'transparent', border: 0, textAlign: 'right' }}>
          <CardHeader
        title={`${data?.totalSales ?? 0} ${data?.currency?.sign ?? '€'}`}
        sx={{ p: 0 }}
        subheader='Gesamtumsatz' />
        </Card>
        <Box flexGrow={1} textAlign="right">
          <PieChart
        colors={cheerfulFiestaPalette}
        slotProps={{ legend: { hidden: true } }}
        series={[
          {
            data: data.salesByYear?.valueLabels?.map((x, i) => ({
          id: x,
          value: data?.salesByYear?.series?.find(serie => serie.key === 'year')?.values?.[i] ?? 0,
          label: x
            })) ?? [],                
            innerRadius: 40,
            outerRadius: 65,
            paddingAngle: 0,
            cornerRadius: 0,
            startAngle: 0,
            endAngle: 360,
            cx: 90,
          },
        ]}
        width={160}
        height={160}
          />
        </Box>
      </Stack>
    </Grid>

    <Grid size={{ xs: 12, sm: 12, md: 12, lg: 8 }} offset={{ lg: -10 }}>
      <LineChart
        sx={{ mt: -10, borderLeft: '1px solid #ddd', borderRight: '1px solid #ddd' }}
        slotProps={{
          legend: { hidden: true },
        }}
        xAxis={[{
          data: data.salesByYear?.valueLabels ?? [], scaleType: 'band', disableTicks: true, disableLine: true,
          tickLabelStyle: { angle: -90, textAnchor: 'end', fontWeight: 400, opacity: 0.5 }
        }]}
        series={[
          { data: salesTotalSeries?.values || [], label: salesTotalSeries?.label || '', color: orange[500] },
          { data: salesYearSeries?.values || [], label: salesYearSeries?.label || '', color: orange[300] },
        ]}
        yAxis={[{ scaleType: 'linear', disableTicks: true, disableLine: true, sx: { display: 'none' } }]}
        height={250}
      />
    </Grid>
  </Grid>;
}

function renderCustomerBlock(data: GetSalesDashboardReadModel) {
  const series1 = data.salesByYear?.series?.filter(x => x.key === 'year').map((serie, index) => ({
    data: serie.values?.map(x => x > 0 ? x : null) ?? [],
    label: serie.label ?? "",
    id: serie.key ?? index,
    stack: '',
  })) ?? [];

  const series2 = data.salesByYear?.series?.filter(x => x.group === 'customer').map((serie, index) => ({
    data: serie.values?.map(x => x > 0 ? x : null) ?? [],
    label: serie.label ?? "",
    id: serie.key ?? index,
    stack: 'total',
  })) ?? [];

  return <Grid container spacing={2} sx={{ mt: 0 }}>
    <Grid size={{ xs: 12, md: 6 }}>
      <Paper sx={{ p: 2 }}>
        <Typography variant="h6">Jahresumsatz pro Kunde</Typography>
        <BarChart
          height={370}
          colors={cheerfulFiestaPalette}
          slotProps={{ legend: { hidden: true } }}
          tooltip={{ trigger: 'item' }}
          series={series1.concat(series2)}
          yAxis={[{ scaleType: 'linear', disableTicks: true, disableLine: true }]}
          xAxis={[{ data: data.salesByYear?.valueLabels ?? [], scaleType: 'band', disableTicks: true }]} />
      </Paper>
    </Grid>

    <Grid size={{ xs: 12, md: 6 }}>
      <Paper sx={{ p: 2 }}>
        <Typography variant="h6">Gesamtumsatz pro Kunde</Typography>
        <BarChart
          dataset={data.salesByCustomer?.map(x => ({ customer: x.name, total: x.total })) ?? []}
          colors={cheerfulFiestaPalette}
          slotProps={{ legend: { hidden: true } }}
          yAxis={[{ scaleType: 'band', dataKey: 'customer' }]}
          series={[{ dataKey: 'total', label: 'Total' }]}
          layout="horizontal"
          height={370}
        />
      </Paper>
    </Grid>
  </Grid>;
}

function render3YearsBlock() {
  return <Grid size={{ xs: 12 }} container spacing={2} sx={{ mt: 2 }}>
    <Grid size={{ xs: 12, sm: 12, lg: 4 }}>
      <Paper sx={{ p: 2, opacity: 1, backgroundColor: blueGrey[200] }}>
        <Typography variant="h6"><TrendingUp fontSize="small" />2024</Typography>
        Coming soon
      </Paper>
    </Grid>
    <Grid size={{ xs: 12, sm: 6, lg: 4 }}>
      <Paper sx={{ p: 2, backgroundColor: blueGrey[100] }}>
        <Typography variant="h6"><TrendingUp fontSize="small" />2023</Typography>
        Coming soon
      </Paper>
    </Grid>
    <Grid size={{ xs: 12, sm: 6, lg: 4 }}>
      <Paper sx={{ p: 2, backgroundColor: blueGrey[50] }}>
        <Typography variant="h6"><TrendingUp fontSize="small" />2022</Typography>
        Coming soon
      </Paper>
    </Grid>
  </Grid>
}

function renderAverageBlock() {
  return <Grid size={{ xs: 12 }} container spacing={2} sx={{ mt: 2 }}>
    <Grid size={{ xs: 12 }}>
      <Paper sx={{ p: 2 }}>
        <Typography variant="h6">Statistik</Typography>
        Coming soon
      </Paper>
    </Grid>
  </Grid>
}

export default SalesDashboard;