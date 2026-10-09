import { useEffect, useState } from "react";
import { Box, Container, Typography, Button } from "@mui/material";
import { Link } from "react-router-dom";
import BookmarkIcon from "@mui/icons-material/Bookmark";
import VacancyCard from "../../components/cards/VacancyCard";
import { fetchFavorites, getLocalFavorites } from "../../services/favoritesService";

export default function Favorites() {
  const [favorites, setFavorites] = useState([]);
  const [loading, setLoading] = useState(true);

  const loadFavorites = async () => {
    setLoading(true);
    try {
      const data = await fetchFavorites();
      setFavorites(data || []);
    } catch {
      setFavorites(getLocalFavorites());
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadFavorites();

    const handleUpdate = () => {
      setFavorites(getLocalFavorites());
    };

    window.addEventListener("favorites-changed", handleUpdate);
    return () => {
      window.removeEventListener("favorites-changed", handleUpdate);
    };
  }, []);

  return (
    <Box sx={{ flexGrow: 1, py: 5 }}>
      <Container maxWidth="lg">
        <Box sx={{ display: "flex", alignItems: "center", gap: 1.5, mb: 1 }}>
          <BookmarkIcon color="primary" sx={{ fontSize: 36 }} />
          <Typography variant="h3" fontWeight="bold">
            Збережені вакансії
          </Typography>
        </Box>
        <Typography color="text.secondary" sx={{ mb: 4 }}>
          {favorites.length} {favorites.length === 1 ? "вакансія" : "вакансій"} у списку обраного
        </Typography>

        {loading ? (
          <Typography color="text.secondary">Завантаження обраного...</Typography>
        ) : favorites.length === 0 ? (
          <Box
            sx={{
              p: 6,
              textAlign: "center",
              border: 1,
              borderColor: "divider",
              borderRadius: 2,
              backgroundColor: "background.paper",
            }}
          >
            <Typography variant="h5" gutterBottom>
              У вас поки немає збережених вакансій
            </Typography>
            <Typography color="text.secondary" sx={{ mb: 3 }}>
              Зберігайте цікаві пропозиції, натискаючи кнопку «В обране», щоб не загубити їх.
            </Typography>
            <Button component={Link} to="/" variant="contained">
              Переглянути доступні вакансії
            </Button>
          </Box>
        ) : (
          <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
            {favorites.map((vacancy) => (
              <VacancyCard key={vacancy.id} vacancy={vacancy} />
            ))}
          </Box>
        )}
      </Container>
    </Box>
  );
}
