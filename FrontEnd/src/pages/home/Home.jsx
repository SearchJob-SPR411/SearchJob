import { useEffect, useState } from "react";
import {
  Box,
  Container,
  TextField,
  Typography,
  InputAdornment,
} from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import VacancyCard from "../../components/cards/VacancyCard";
import { api } from "../../api";

export default function Home() {
  const [query, setQuery] = useState("");
  const [vacancies, setVacancies] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchVacancies = async () => {
      try {
        const response = await api.get("Vacancies");
        setVacancies(response.data);
      } catch (error) {
        console.error("Failed to load vacancies:", error);
        setError("Failed to load vacancies. Please try again later.");
      } finally {
        setLoading(false);
      }
    };

    fetchVacancies();
  }, []);

  const searchQuery = query.trim().toLowerCase();
  const filteredVacancies = vacancies.filter(
  (vacancy) =>
    vacancy.status === "Active" &&
    (
      vacancy.title +
      vacancy.companyName +
      vacancy.location
    )
      .toLowerCase()
      .includes(searchQuery)
);

  return (
    <Box sx={{ flexGrow: 1 }}>
      <Box
        sx={{
          py: 8,
          backgroundColor: "background.paper",
        }}
      >
        <Container maxWidth="lg">
          <Typography variant="h2" gutterBottom>
            Find your next job
          </Typography>

          <Typography
            variant="h6"
            color="text.secondary"
            sx={{ mb: 4 }}
          >
            Browse open vacancies from companies hiring right now.
          </Typography>

          <TextField
            fullWidth
            value={query}
            onChange={(event) => setQuery(event.target.value)}
            placeholder="Search by title, company or location"
            slotProps={{
              input: {
                startAdornment: (
                  <InputAdornment position="start">
                    <SearchIcon />
                  </InputAdornment>
                ),
              },
            }}
          />
        </Container>
      </Box>

      <Container maxWidth="lg" sx={{ py: 5 }}>
        <Box
          sx={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
            mb: 3,
          }}
        >
          <Typography variant="h4">
            Open vacancies
          </Typography>

          <Typography color="text.secondary">
            {filteredVacancies.length} results
          </Typography>
        </Box>

        <Box
          sx={{
            display: "flex",
            flexDirection: "column",
            gap: 2,
          }}
        >
          {loading ? (
            <Typography color="text.secondary">
              Loading vacancies...
            </Typography>
          ) : error ? (
            <Typography color="error">
              {error}
            </Typography>
          ) : (
            <>
              {filteredVacancies.map((vacancy) => (
                <VacancyCard
                  key={vacancy.id}
                  vacancy={vacancy}
                />
              ))}

              {filteredVacancies.length === 0 && (
                <Typography color="text.secondary">
                  No vacancies match "{query}".
                </Typography>
              )}
            </>
          )}
        </Box>
      </Container>
    </Box>
  );
}