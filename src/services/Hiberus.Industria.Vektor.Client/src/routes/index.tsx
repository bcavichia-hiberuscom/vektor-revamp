import { createFileRoute } from "@tanstack/react-router";
import VektorMap from "@/components/Map/VektorMap";

export const Route = createFileRoute("/")({
  component: HomePage,
});

function HomePage() {
  return (
    <div style={{ height: "100vh", width: "100vw" }}>
      <VektorMap />
    </div>
  );
}
