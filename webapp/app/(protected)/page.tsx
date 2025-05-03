import { Button } from "@/components/ui/button";

export default function Home() {
  return (
    <div className="flex flex-col items-center justify-center h-screen">
      <h1 className="text-4xl font-bold p-4">
        Should only be accessible if logged in!
      </h1>
      <Button>Click meeee</Button>
    </div>
  );
}
