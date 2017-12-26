# NumberValidators
中国大陆证件、号码的验证类库，目前包含身份证、增值税发票、工商注册码/统一社会信用代码

## .NET版本支持
目前支持以下版本：`.NET40`、`.NET Standard 2.0`

## 规范约束`IValidator<out T>`
所有验证均需实现`IValidator<out T>`接口，其中泛型`T`具备如下约束：必须继承自`ValidationResult`且需有无参构造函数，该接口定义了以下规范：
* 随机生成一个符合规则的号码 `string GenerateRandomNumber();`
* 验证号码是否正确 `T Validate(string number);`

实际验证会在此接口基础上定义额外的规范约束，且每种证件号码均默认提供`ValidatorHelper`类，用于统一验证该类型的证件号码，根据实际调用的`IValidator<out T>`实现，返回的`ValidationResult`也可能会有所不同

PS：如果验证结果`IsValid`为True，那么相应的`ValidationResult`会包含该号码可识别的所有信息

## 身份证
默认提供`ID15Validator`（15位旧身份证）以及`ID18Validator`（18位新身份证）两种类型的身份证验证，具体使用代码如下：
```csharp
valid = new ID15Validator().Validate(idNumber); //旧身份证验证
valid = new ID18Validator().Validate(idNumber); //新身份证验证
valid = IDValidatorHelper.Validate(idNumber); //无法确认是哪种身份证时可以通过该类进行验证
```

## 增值税发票
默认提供`VATCode10Validator`（增值税专用发票、增值税普通发票、货物运输业增值税专用发票）以及`VATCode12Validator`（增值税普通发票[卷票]、增值税电子普通发票）两种长度共五中类型的增值税发票验证（货物运输业增值税专用发票按国家规定目前已停用），具体使用代码如下：
```csharp
valid = new VATCode10Validator().Validate(vatCode); //增值税专用发票、增值税普通发票、货物运输业增值税专用发票验证
valid = new VATCode12Validator().Validate(vatCode); //增值税普通发票[卷票]、增值税电子普通发票验证
valid = VATCodeValidatorHelper.Validate(vatCode); //无法确认是哪种增值税发票时可以通过该类进行验证
```
注意`VATCode10Validator`返回验证结果为`VATCode10ValidationResult`，`VATCode12Validator`返回验证结果为`VATCodeValidationResult`，`VATCodeValidatorHelper`返回验证结果为`VATCodeValidationResult`（实际也可能为`VATCode10ValidationResult`）

## 工商注册码/统一社会信用代码
默认提供`RegistrationNo15Validator`（工商注册码）以及`RegistrationNo18Validator`（法人和其他组织统一社会信用代码），具体使用代码如下：
```csharp
valid = new RegistrationNo15Validator().Validate(code); //工商注册码验证
valid = new RegistrationNo18Validator().Validate(code); //法人和其他组织统一社会信用代码验证
valid = RegistrationNoValidatorHelper.Validate(code); //无法确认是工商注册码还是法人和其他组织统一社会信用代码时可以通过该类进行验证
```
注意`RegistrationNo15Validator`返回验证结果为`RegistrationNo15ValidationResult`，`RegistrationNo18Validator`返回验证结果为`RegistrationNo18ValidationResult`，`RegistrationNoValidatorHelper`返回验证结果为`RegistrationNoValidationResult`（实际也可能为`RegistrationNo15ValidationResult`或`RegistrationNo18ValidationResult`）
