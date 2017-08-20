using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ARMD.DataContracts.Api.ReferenceData.Interfaces;
using ARMD.DataContracts.Api.ReferenceData.ReferenceDataVersionTypesContainers;
using ARMD.DataContracts.FromStations.Events.CPPKTicket;
using ARMD.DataContracts.ToStations.ReferenceData.AccessSchemes;
using ARMD.DataContracts.ToStations.ReferenceData.DevicesAgents;
using ARMD.DataContracts.ToStations.ReferenceData.RatesRoutes;
using ARMD.DataContracts.ToStations.ReferenceData.ScheduleInfo;
using ARMD.DataContracts.ToStations.ReferenceData.StationsCarriers;
using ARMD.DataContracts.ToStations.ReferenceData.SupportingTables;
using ARMD.DataContracts.ToStations.ReferenceData.TicketExemptions;
using NLog;
using TestApp.JsonUtils;
using Version = ARMD.DataContracts.ToStations.ReferenceData.StationsCarriers.Version;

namespace TestApp.Models
{
    class BaseReferenceDataVersionTablesProxy1 : IBaseReferenceDataVersionTables, IDisposable    
    {
        //private readonly string _tempDirectoryName;
        private CollectionDeserializer _collectionDeserializer;
        //private readonly JsonFileSplitter _fileSplitter;
        //private List<string> _fileNames;
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public BaseReferenceDataVersionTablesProxy1(string fileName/*, string tempDirectoryName = null*/)
        {
            //_tempDirectoryName = tempDirectoryName ?? Path.GetDirectoryName(fileName);
            //_fileSplitter = new JsonFileSplitter(typeof(BaseReferenceDataVersionTables), fileName, _tempDirectoryName);
        }

        public void Initialize()
        {
            /*var typeInfos = _fileSplitter.SplitIntoFiles();
            _fileNames = typeInfos.Select(x => Path.Combine(_tempDirectoryName, x.Value)).ToList();*/
            _collectionDeserializer = new CollectionDeserializer(/*typeInfos, _tempDirectoryName*/null, null);
        }

        public IEnumerable<Region> Regions => _collectionDeserializer.Deserialize<Region>();

        public IEnumerable<Carrier> Carriers => _collectionDeserializer.Deserialize<Carrier>();

        public IEnumerable<Railway> Railways => _collectionDeserializer.Deserialize<Railway>();

        public IEnumerable<Direction> Directions => _collectionDeserializer.Deserialize<Direction>();

        public IEnumerable<CppkSubdivision> CppkSubdivisions => _collectionDeserializer.Deserialize<CppkSubdivision>();

        public IEnumerable<ProductionSection> ProductionSections => _collectionDeserializer.Deserialize<ProductionSection>();

        public IEnumerable<TrainCategory> TrainCategories => _collectionDeserializer.Deserialize<TrainCategory>();

        public IEnumerable<CarClass> CarClasses => _collectionDeserializer.Deserialize<CarClass>();

        public IEnumerable<TariffPlan> TariffPlans => _collectionDeserializer.Deserialize<TariffPlan>();

        public IEnumerable<TariffsLimit> TariffsLimits => _collectionDeserializer.Deserialize<TariffsLimit>();

        public IEnumerable<ProcessingFee> ProcessingFees => _collectionDeserializer.Deserialize<ProcessingFee>();

        public IEnumerable<ServiceFee> ServiceFees => _collectionDeserializer.Deserialize<ServiceFee>();

        public IEnumerable<DepositTariff> DepositTariffs => _collectionDeserializer.Deserialize<DepositTariff>();

        public IEnumerable<Station> Stations => _collectionDeserializer.Deserialize<Station>();

        public IEnumerable<StationForProductionSection> StationForProductionSections =>
            _collectionDeserializer.Deserialize<StationForProductionSection>();

        public IEnumerable<StationToTariffZone> StationsToTariffZones => _collectionDeserializer.Deserialize<StationToTariffZone>();

        public IEnumerable<RouteToTariffPlan> RouteToTariffPlans => _collectionDeserializer.Deserialize<RouteToTariffPlan>();

        public IEnumerable<Route> Routes => _collectionDeserializer.Deserialize<Route>();

        public IEnumerable<StationsOnRoute> StationsOnRoutes => _collectionDeserializer.Deserialize<StationsOnRoute>();

        public IEnumerable<TicketStorageType> TicketStorageTypes => _collectionDeserializer.Deserialize<TicketStorageType>();

        public IEnumerable<TicketStorageTypeToTicketType> TicketStorageTypesToTicketTypes =>
            _collectionDeserializer.Deserialize<TicketStorageTypeToTicketType>();

        public IEnumerable<ExpressTicketCategory> ExpressTicketCategories => _collectionDeserializer.Deserialize<ExpressTicketCategory>();

        public IEnumerable<ExpressTicketType> ExpressTicketTypes => _collectionDeserializer.Deserialize<ExpressTicketType>();

        public IEnumerable<TicketCategory> TicketCategories => _collectionDeserializer.Deserialize<TicketCategory>();

        public IEnumerable<TicketCategoryTranslate> TicketCategoryTranslates =>
            _collectionDeserializer.Deserialize<TicketCategoryTranslate>();

        public IEnumerable<TicketType> TicketTypes => _collectionDeserializer.Deserialize<TicketType>();

        public IEnumerable<TicketTypeTranslate> TicketTypeTranslates => _collectionDeserializer.Deserialize<TicketTypeTranslate>();

        public IEnumerable<TicketStopListReason> TicketStopListReasons => _collectionDeserializer.Deserialize<TicketStopListReason>();

        public IEnumerable<PaymentMethod> PaymentMethods => _collectionDeserializer.Deserialize<PaymentMethod>();

        public IEnumerable<TicketTypesToDeviceType> TicketTypesToDeviceTypes => _collectionDeserializer.Deserialize<TicketTypesToDeviceType>();

        public IEnumerable<ValidityPeriod> ValidityPeriods => _collectionDeserializer.Deserialize<ValidityPeriod>();

        public IEnumerable<TicketTypeValidityTime> TicketTypeValidityTimes => _collectionDeserializer.Deserialize<TicketTypeValidityTime>();

        public IEnumerable<Tariff> Tariffs => _collectionDeserializer.Deserialize<Tariff>();

        public IEnumerable<ExemptionGroup> ExemptionGroups => _collectionDeserializer.Deserialize<ExemptionGroup>();

        public IEnumerable<ExemptionOrganization> ExemptionOrganizations => _collectionDeserializer.Deserialize<ExemptionOrganization>();

        public IEnumerable<Exemption> Exemptions => _collectionDeserializer.Deserialize<Exemption>();

        public IEnumerable<ExemptionTo> ExemptionTo => _collectionDeserializer.Deserialize<ExemptionTo>();

        public IEnumerable<ExemptionToRegion> ExemptionsToRegions => _collectionDeserializer.Deserialize<ExemptionToRegion>();

        public IEnumerable<ProhibitedForManualEntryExemption> ProhibitedForManualEntryExemptions =>
            _collectionDeserializer.Deserialize<ProhibitedForManualEntryExemption>();

        public IEnumerable<BannedDeviceExemption> BannedDeviceExemptions => _collectionDeserializer.Deserialize<BannedDeviceExemption>();

        public IEnumerable<ExemptionBannedForTariffPlan> ExemptionsBannedForTariffPlans =>
            _collectionDeserializer.Deserialize<ExemptionBannedForTariffPlan>();

        public IEnumerable<ExemptionCategory> ExemptionsCategories => _collectionDeserializer.Deserialize<ExemptionCategory>();

        public IEnumerable<ProhibitedTicketTypeForExemptionCategory> ProhibitedTicketTypeForExemptionsCategories =>
            _collectionDeserializer.Deserialize<ProhibitedTicketTypeForExemptionCategory>();

        public IEnumerable<DeviceType> DeviceTypes => _collectionDeserializer.Deserialize<DeviceType>();

        public IEnumerable<SmartCardType> SmartCardTypes => _collectionDeserializer.Deserialize<SmartCardType>();

        public IEnumerable<SmartCardAttendantReason> SmartCardAttendantReasons => _collectionDeserializer.Deserialize<SmartCardAttendantReason>();

        public IEnumerable<Calendar> Calendars => _collectionDeserializer.Deserialize<Calendar>();

        public IEnumerable<Language> Languages => _collectionDeserializer.Deserialize<Language>();

        public IEnumerable<RegionCalendar> RegionCalendars => _collectionDeserializer.Deserialize<RegionCalendar>();

        public IEnumerable<HolidayCalendar> HolidayCalendars => _collectionDeserializer.Deserialize<HolidayCalendar>();

        public IEnumerable<HolidayTransfer> HolidayTransfers => _collectionDeserializer.Deserialize<HolidayTransfer>();

        public IEnumerable<TariffZone> TariffZones => _collectionDeserializer.Deserialize<TariffZone>();

        public IEnumerable<TicketTypeInTariffZone> TicketTypesInTariffZones =>
            _collectionDeserializer.Deserialize<TicketTypeInTariffZone>();

        public IEnumerable<SmartCardIssuer> SmartCardIssuers => _collectionDeserializer.Deserialize<SmartCardIssuer>();

        public IEnumerable<SmartCardStopListReason> SmartCardStopListReasons => _collectionDeserializer.Deserialize<SmartCardStopListReason>();

        public IEnumerable<SmartCardStopListReasonTranslate> SmartCardStopListReasonTranslates =>
            _collectionDeserializer.Deserialize<SmartCardStopListReasonTranslate>();

        public IEnumerable<SmartCardStopListReasonsFromIssuerToInner> SmartCardStopListReasonsFromIssuerToInner =>
            _collectionDeserializer.Deserialize<SmartCardStopListReasonsFromIssuerToInner>();

        public IEnumerable<SmartCardCancellationReason> SmartCardCancellationReasons =>
            _collectionDeserializer.Deserialize<SmartCardCancellationReason>();

        public IEnumerable<RegulatoryDocument> RegulatoryDocuments => _collectionDeserializer.Deserialize<RegulatoryDocument>();

        public IEnumerable<RegulatoryDocumentTranslate> RegulatoryDocumentTranslates => _collectionDeserializer.Deserialize<RegulatoryDocumentTranslate>();

        public IEnumerable<RegulatoryDocumentChapter> RegulatoryDocumentChapters => _collectionDeserializer.Deserialize<RegulatoryDocumentChapter>();

        public IEnumerable<RegulatoryDocumentChapterTranslate> RegulatoryDocumentChapterTranslates =>
            _collectionDeserializer.Deserialize<RegulatoryDocumentChapterTranslate>();

        public IEnumerable<ExemptionBannedForTicketStorageType> ExemptionBannedForTicketStorageTypes =>
            _collectionDeserializer.Deserialize<ExemptionBannedForTicketStorageType>();

        public IEnumerable<SalesChannel> SalesChannels => _collectionDeserializer.Deserialize<SalesChannel>();

        public IEnumerable<CredentialDocumentType> CredentialDocumentTypes => _collectionDeserializer.Deserialize<CredentialDocumentType>();

        public IEnumerable<GypType> GypTypes => _collectionDeserializer.Deserialize<GypType>();

        public IEnumerable<AcquiringBank> AcquiringBanks => _collectionDeserializer.Deserialize<AcquiringBank>();

        public IEnumerable<Fine> Fines
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<SmartCardIssuerForTicketStorageType> SmartCardIssuerForTicketStorageTypes =>
            _collectionDeserializer.Deserialize<SmartCardIssuerForTicketStorageType>();

        public IEnumerable<StopListIssuerTypeCriteria> StopListIssuerTypeCriteria =>
            _collectionDeserializer.Deserialize<StopListIssuerTypeCriteria>();

        public IEnumerable<Tax> Taxes => _collectionDeserializer.Deserialize<Tax>();

        public IEnumerable<MifareKey> MifareKeys => _collectionDeserializer.Deserialize<MifareKey>();

        public IEnumerable<TurnstilePassageResult> TurnstilePassageResults =>
            _collectionDeserializer.Deserialize<TurnstilePassageResult>();

        public IEnumerable<AccessScheme> AccessSchemes => _collectionDeserializer.Deserialize<AccessScheme>();

        public IEnumerable<AccessRule> AccessRules => _collectionDeserializer.Deserialize<AccessRule>();

        public IEnumerable<OtherTransportOperator> OtherTransportOperators => _collectionDeserializer.Deserialize<OtherTransportOperator>();

        public IEnumerable<OtherTransportOperatorsTicketCategory> OtherTransportOperatorsTicketCategories =>
            _collectionDeserializer.Deserialize<OtherTransportOperatorsTicketCategory>();

        public IEnumerable<OtherTransportOperatorsTicketCategoryTranslate> OtherTransportOperatorsTicketCategoryTranslates =>
            _collectionDeserializer.Deserialize<OtherTransportOperatorsTicketCategoryTranslate>();

        public IEnumerable<OtherTransportOperatorsTicketSubCategory> OtherTransportOperatorsTicketSubCategories =>
            _collectionDeserializer.Deserialize<OtherTransportOperatorsTicketSubCategory>();

        public IEnumerable<OtherTransportOperatorsTicketSubCategoryTranslate> OtherTransportOperatorsTicketSubCategoryTranslates =>
            _collectionDeserializer.Deserialize<OtherTransportOperatorsTicketSubCategoryTranslate>();

        public IEnumerable<DeviceModel> DeviceModels => _collectionDeserializer.Deserialize<DeviceModel>();

        public IEnumerable<ProductionSectionForUkk> ProductionSectionsForUkk => _collectionDeserializer.Deserialize<ProductionSectionForUkk>();

        public IEnumerable<StationRushHour> StationRushHours => _collectionDeserializer.Deserialize<StationRushHour>();

        public IEnumerable<StationSeasonalTimetable> StationSeasonalTimetables => _collectionDeserializer.Deserialize<StationSeasonalTimetable>();

        public IEnumerable<StationWorkInterval> StationWorkIntervals => _collectionDeserializer.Deserialize<StationWorkInterval>();

        public IEnumerable<Version> Versions => _collectionDeserializer.Deserialize<Version>();

        public IEnumerable<MoneyEntryType> MoneyEntryTypes => _collectionDeserializer.Deserialize<MoneyEntryType>();

        public IEnumerable<MoneyEntrySubType> MoneyEntrySubTypes => _collectionDeserializer.Deserialize<MoneyEntrySubType>();

        public IEnumerable<CalculationFeeForReturnAbonement> CalculationFeeForReturnAbonements =>
            _collectionDeserializer.Deserialize<CalculationFeeForReturnAbonement>();

        public IEnumerable<TariffPlansRelationForReturnAbonement> TariffPlansRelationForReturnAbonement =>
            _collectionDeserializer.Deserialize<TariffPlansRelationForReturnAbonement>();

        public IEnumerable<TrainMotionMode> TrainMotionModes => _collectionDeserializer.Deserialize<TrainMotionMode>();

        public IEnumerable<TrainMotionModeTranslate> TrainMotionModeTranslates => _collectionDeserializer.Deserialize<TrainMotionModeTranslate>();

        //TODO: подумать
        public int OveralCount()
        {
            return 1;
        }

        public void Dispose()
        {
            //_fileSplitter.DeleteFiles()
        }

        /*public void Initialize(string fileName)
        {
            _collectionPrоvider = new CollectionPrоvider<BaseReferenceDataVersionTables>(fileName);
        }*/
    }
}
