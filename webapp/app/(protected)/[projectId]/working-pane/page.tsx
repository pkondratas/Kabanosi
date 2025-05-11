import { WorkingPane } from "@/components/features/protected/working-pane/working-pane";

export default function Page() {
	return (
		<div className="flex min-h-screen w-full items-start justify-center p-6 md:p-10">
			<div className="w-full max-w-screen-lg pt-[70px]">
				<WorkingPane />
			</div>
		</div>
	);
};