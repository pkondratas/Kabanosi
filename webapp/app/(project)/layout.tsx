import { LoggedInNavbar } from "@/components/features/protected/shared/loggedInNavbar";

export default function ProtectedLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <>
      <header className="fixed top-0 w-full z-50">
        <LoggedInNavbar />
      </header>
      <main>{children}</main>
    </>
  );
}
