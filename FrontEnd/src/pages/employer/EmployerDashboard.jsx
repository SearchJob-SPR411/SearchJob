import { useState, useEffect } from "react";
import { Container, Typography, Box, Card, CardContent, CircularProgress, Accordion, AccordionSummary, AccordionDetails, Button } from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { api } from "../../api";
import { Link } from "react-router-dom";

export default function EmployerDashboard() {
    const [companies, setCompanies] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const recruiterUserId = 1; 

    useEffect(() => {
        const fetchDashboardData = async () => {
            try {
                setLoading(true);
                const compRes = await api.get(`/Companies/user/${recruiterUserId}`);
                const comps = compRes.data;

                const compsWithVacancies = await Promise.all(comps.map(async (c) => {
                    const vacRes = await api.get(`/Vacancies/company/${c.id}`);
                    const vacancies = vacRes.data;

                    const vacsWithApps = await Promise.all(vacancies.map(async (v) => {
                        const appRes = await api.get(`/applications/vacancy/${v.id}?recruiterUserId=${recruiterUserId}`);
                        return { ...v, applications: appRes.data };
                    }));

                    return { ...c, vacancies: vacsWithApps };
                }));

                setCompanies(compsWithVacancies);
            } catch (err) {
                console.error(err);
                setError("Failed to load dashboard data.");
            } finally {
                setLoading(false);
            }
        };

        fetchDashboardData();
    }, []);

    if (loading) {
        return (
            <Container sx={{ mt: 4, textAlign: "center" }}>
                <CircularProgress />
            </Container>
        );
    }

    if (error) {
        return (
            <Container sx={{ mt: 4 }}>
                <Typography color="error">{error}</Typography>
            </Container>
        );
    }

    return (
        <Container sx={{ mt: 4, mb: 4 }}>
            <Typography variant="h4" gutterBottom>
                Employer Dashboard
            </Typography>

            {companies.length === 0 ? (
                <Typography>You have no companies yet.</Typography>
            ) : (
                companies.map(company => (
                    <Card key={company.id} sx={{ mb: 3 }}>
                        <CardContent>
                            <Typography variant="h5" gutterBottom>
                                {company.name}
                            </Typography>
                            <Typography variant="body2" color="text.secondary" gutterBottom>
                                {company.description}
                            </Typography>

                            <Typography variant="h6" sx={{ mt: 2, mb: 1 }}>
                                Vacancies
                            </Typography>
                            
                            {company.vacancies.length === 0 ? (
                                <Typography variant="body2">No vacancies posted yet.</Typography>
                            ) : (
                                company.vacancies.map(vacancy => (
                                    <Accordion key={vacancy.id}>
                                        <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                                            <Typography sx={{ fontWeight: "bold", width: '50%' }}>
                                                {vacancy.title}
                                            </Typography>
                                            <Typography sx={{ color: 'text.secondary' }}>
                                                {vacancy.applications?.length || 0} applications
                                            </Typography>
                                        </AccordionSummary>
                                        <AccordionDetails>
                                            <Box sx={{ mb: 2 }}>
                                                <Button 
                                                    component={Link} 
                                                    to={`/vacancies/${vacancy.id}/applications`} 
                                                    variant="outlined" 
                                                    size="small"
                                                >
                                                    Manage Applications
                                                </Button>
                                            </Box>
                                            {vacancy.applications?.length === 0 ? (
                                                <Typography variant="body2">No applications yet.</Typography>
                                            ) : (
                                                vacancy.applications?.map(app => (
                                                    <Box key={app.id} sx={{ mb: 1, p: 1, border: '1px solid #eee', borderRadius: 1 }}>
                                                        <Typography variant="body2">
                                                            <strong>Applicant ID:</strong> {app.userId} <br/>
                                                            <strong>Status:</strong> {app.status} <br/>
                                                            <strong>Applied on:</strong> {new Date(app.appliedAt).toLocaleDateString()}
                                                        </Typography>
                                                    </Box>
                                                ))
                                            )}
                                        </AccordionDetails>
                                    </Accordion>
                                ))
                            )}
                        </CardContent>
                    </Card>
                ))
            )}
        </Container>
    );
}
