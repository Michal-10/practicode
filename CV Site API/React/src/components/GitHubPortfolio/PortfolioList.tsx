
import { Grid, Card, CardContent, CardActions, Typography, Chip, Box, Button, Divider } from "@mui/material"
import StarIcon from "@mui/icons-material/Star"
import LaunchIcon from "@mui/icons-material/Launch"

type Repo = {
  id: number
  name: string
  description: string
  html_url: string
  stargazers_count: number
  language: string
}

type Props = {
  repos: Repo[]
}

// Language color mapping
const languageColors: Record<string, string> = {
  JavaScript: "#f1e05a",
  TypeScript: "#3178c6",
  Python: "#3572A5",
  Java: "#b07219",
  "C#": "#178600",
  PHP: "#4F5D95",
  Ruby: "#701516",
  Go: "#00ADD8",
  Swift: "#F05138",
  Kotlin: "#A97BFF",
  Rust: "#DEA584",
  Dart: "#00B4AB",
  CSS: "#563d7c",
  HTML: "#e34c26",
}

export default function PortfolioList({ repos }: Props) {
  if (!repos.length)
    return (
      <Box
        sx={{
          textAlign: "center",
          p: 4,
          borderRadius: 2,
          bgcolor: "rgba(138, 51, 253, 0.03)",
        }}
      >
        <Typography variant="h6" sx={{ color: "rgba(138, 51, 253, 0.7)" }}>
          No repositories found
        </Typography>
      </Box>
    )

  return (
    <Grid container spacing={3}>
      {repos.map((repo) => (
        <Grid item xs={12} sm={6} key={repo.id}>
          <Card
            sx={{
              height: "100%",
              display: "flex",
              flexDirection: "column",
              transition: "all 0.3s ease-in-out",
              borderRadius: 2,
              border: "1px solid rgba(138, 51, 253, 0.1)",
              boxShadow: "0 4px 12px rgba(138, 51, 253, 0.05)",
              "&:hover": {
                transform: "translateY(-4px)",
                boxShadow: "0 12px 20px rgba(138, 51, 253, 0.1)",
                borderColor: "rgba(138, 51, 253, 0.3)",
              },
            }}
          >
            <CardContent sx={{ flexGrow: 1 }}>
              
              <Typography variant="h6" component="h3" gutterBottom noWrap sx={{ color: "#8a33fd" }}>
                {repo.name}
      
              </Typography>

              <Typography
                variant="body2"
                color="text.secondary"
                sx={{
                  mb: 2,
                  display: "-webkit-box",
                  WebkitLineClamp: 2,
                  WebkitBoxOrient: "vertical",
                  overflow: "hidden",
                  minHeight: "40px",
                }}
              >
                {repo.description || "No description available"}
              </Typography>

              <Box sx={{ display: "flex", alignItems: "center", gap: 1, mt: 2 }}>
                <Chip
                  icon={<StarIcon fontSize="small" sx={{ color: "#f9a825" }} />}
                  label={repo.stargazers_count}
                  size="small"
                  variant="outlined"
                  sx={{
                    borderColor: "rgba(249, 168, 37, 0.5)",
                    color: "#f9a825",
                  }}
                />

                {repo.language && (
                  <Chip
                    icon={
                      <Box
                        component="span"
                        sx={{
                          width: 10,
                          height: 10,
                          borderRadius: "50%",
                          bgcolor: languageColors[repo.language] || "#6e6e6e",
                          display: "inline-block",
                          mr: 0.5,
                        }}
                      />
                    }
                    label={repo.language}
                    size="small"
                    variant="outlined"
                    sx={{ borderColor: "rgba(138, 51, 253, 0.2)" }}
                  />
                )}
              </Box>
            </CardContent>

            <Divider sx={{ borderColor: "rgba(138, 51, 253, 0.1)" }} />

            <CardActions>
              {/* Fixed GitHub link - using Button instead of Link for better reliability */}
              <Button
                href={`https://github.com/Michal-10/${repo.name}`}
                target="_blank"
                rel="noopener noreferrer"
                startIcon={<LaunchIcon />}
                sx={{
                  color: "#8a33fd",
                  fontSize: 14,
                  textTransform: "none",
                  "&:hover": {
                    backgroundColor: "rgba(138, 51, 253, 0.04)",
                    color: "#7928da",
                  },
                }}
              >
                View on GitHub
              </Button>
              
            </CardActions>
          </Card>
        </Grid>
      ))}
    </Grid>
  )
}
